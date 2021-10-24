import { Logger } from "@tsed/logger";

declare global {
	interface Function {
		logger: Logger;
	}
}

export const Log = (logger: Logger) => (target: any, propertyKey: string, descriptor: PropertyDescriptor) => {
	let originalMethod = descriptor.value;

	descriptor.value = function (...args: any[]) {
		logger.debug(`${propertyKey} - Entering`);

		const now = Date.now();
		const result = originalMethod.apply(this, args);

		const exitLog = () => {
			logger.debug(`${propertyKey} - Exited after ${Date.now() - now}ms`);
		};

		if (typeof result === "object" && typeof result.then === "function") {
			const promise = result.then(exitLog);
			if (typeof promise.catch === "function") {
				promise.catch((e: any) => e);
			}
		} else {
			exitLog();
		}

		return result;
	};
};
