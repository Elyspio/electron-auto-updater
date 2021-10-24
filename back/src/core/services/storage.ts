import { promises } from "fs";
import * as path from "path";
import * as os from "os";
import { Log } from "../utils/decorators/logger";
import { getLogger } from "../utils/logger";

const { writeFile, readFile } = promises;

export const files = {};

export class Storage {
	private static logger = getLogger.service(Storage);

	@Log(Storage.logger)
	async store(name: string, data: string | object) {
		if (name[0] === "~") {
			name = path.join(os.homedir(), name.slice(1));
		}

		if (typeof data === "object") data = JSON.stringify(data, null, 4);

		return writeFile(path.resolve(name), data);
	}

	@Log(Storage.logger)
	async read(name: string) {
		return (await readFile(name)).toString();
	}
}
