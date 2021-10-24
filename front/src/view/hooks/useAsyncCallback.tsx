import * as React from "react";

export interface AsyncCallbackStatus {
	/**
	 * True while the async action is being executed.
	 */
	isExecuting: boolean;

	/**
	 * Error raised by the callback.
	 */
	error: any;

	/**
	 * Flag set to true if the last invocation of the scallback executed successfully (i.e. without throwing any exceptions).
	 */
	successfullyExecuted: boolean;
}

export function useAsyncCallback<T extends (...args: any[]) => Promise<any>>(callback: T, deps: React.DependencyList): [T, AsyncCallbackStatus] {
	const [isExecuting, setIsExecuting] = React.useState<boolean>(false);
	const [error, setError] = React.useState<any>(undefined);
	const [success, setSuccess] = React.useState<boolean>(false);

	const wrappedCallback: T = React.useCallback(
		async (...argsx: any[]) => {
			setIsExecuting(true);
			setSuccess(false);

			try {
				let ret = await callback(...argsx);
				setSuccess(true);
				setError(undefined);
				setIsExecuting(false);
				return ret;
			} catch (e) {
				setError(e);
				setIsExecuting(false);
				throw e;
			}
			// eslint-disable-next-line react-hooks/exhaustive-deps
		},
		[...deps, setIsExecuting, setError, callback]
	) as T;

	return [wrappedCallback, { isExecuting, error, successfullyExecuted: success }];
}
