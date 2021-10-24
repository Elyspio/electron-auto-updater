import "@tsed/platform-express"; // /!\ keep this import
import { PlatformExpress } from "@tsed/platform-express";
import { Server } from "./web/server";
import { getLogger } from "./core/utils/logger";

if (require.main === module) {
	bootstrap();
}

async function bootstrap() {
	const logger = getLogger.default();

	try {
		logger.debug("Start server...");
		const platform = await PlatformExpress.bootstrap(Server, {});

		await platform.listen();
		logger.debug("Server initialized");
	} catch (er) {
		logger.error(er);
	}
}
