import * as path from "path";
import {applicationPath} from "../../config/appStorage";
import {ensureDir, ensureDirSync, lstat, readdir, writeFile} from "fs-extra";
import {Platform} from "../../web/controllers/appStorage/appStorage";
import {Services} from "./index";
import {Log} from "../utils/decorators/logger";
import {getLogger} from "../utils/logger";

const metadata = "metadata.json"


export type Version = {
	val: string,
	date: Date
}


export type Metadata = {
	version: {
		windows: Version[],
		linux: Version[]
	}
}


ensureDirSync(applicationPath);

export class AppStorageService {


	private static logger = getLogger.service(AppStorageService);

	@Log(AppStorageService.logger)
	getAppPath(app: string) {
		return path.join(applicationPath, app);
	}

	@Log(AppStorageService.logger)
	async readMetadata(app: string): Promise<Metadata> {

		let appPath = path.join(this.getAppPath(app), metadata);
		try {
			const json = (await Services.storage.read(appPath)).toString();
			return JSON.parse(json);
		} catch (e) {
			const newData = {
				version: {
					linux: [],
					windows: []
				}
			};

			await Services.storage.store(appPath, newData);

			return newData
		}
	}

	@Log(AppStorageService.logger)
	async storeApp(app: string, version: string, platform: Platform, data: number[]) {
		const appPath = this.getAppPath(app);
		await ensureDir(appPath);
		await writeFile(path.join(appPath, version + (platform === "windows" ? ".exe" : "")), Buffer.from(data))

		const meta = await this.readMetadata(app);
		meta.version[platform].push({val: version, date: new Date()});
		await Services.storage.store(path.join(appPath, metadata), meta);
	}

	@Log(AppStorageService.logger)
	async getLatestVersion(app: string, platform: Platform) {
		const json = await this.readMetadata(app)
		return json.version[platform][json.version[platform].length - 1];
	}


	@Log(AppStorageService.logger)
	async getLatest(app: string, platform: string | Platform) {
		const files = await readdir(this.getAppPath(app));

		const platformApps = files
			.filter(x => x !== metadata)
			.filter(app =>
				platform === "windows"
					? app.endsWith(".exe")
					: !app.endsWith(".exe")
			)

		const filesWithMtime = await Promise.all(platformApps.map(async f => {
			let filePath = path.join(this.getAppPath(app), f);
			return ({path: filePath, mtime: (await lstat(filePath)).mtimeMs});
		}));

		const latest = filesWithMtime.reduce((prev, current) => {
			if (current.mtime > prev.mtime) return current;
			return prev;
		})

		return Services.storage.read(latest.path);
	}

	@Log(AppStorageService.logger)
	async getVersions(app: string) {
		const {version} = await this.readMetadata(app);
		return version;
	}

	@Log(AppStorageService.logger)
	async getApps() {
		return await readdir(applicationPath);
	}

	@Log(AppStorageService.logger)
	async getBinary(app: string, platform: Platform, version: string) {
		const p = path.join(this.getAppPath(app), `${version}${platform === "windows" ? ".exe" : ""}`);
		return Services.storage.read(p);
	}


}
