import { inject, injectable } from "inversify";
import { AppsApiClient } from "../apis/backend";
import { DiKeysApi } from "../di/apis/di.keys.api";
import { AppArch, AppVersion } from "../apis/backend/generated";

@injectable()
export class AppsService {
	@inject(DiKeysApi.apps)
	private api!: AppsApiClient;

	constructor() {
		console.log("Constructor");
	}

	public getAppsName() {
		return this.api.client.getApps().then(res => res.data);
	}

	public async getAppVersions(name: string) {
		const allMetadata = await this.api.client.getAllMetadata(name).then(res => res.data);

		const ret: Record<AppArch, AppVersion[]> = {
			Win32: [],
			Win64: [],
			LinuxDeb: [],
			LinuxRpm: [],
			LinuxSnap: [],
		};

		for (let metadata of allMetadata) {
			ret[metadata.arch].push(metadata.version);
		}

		return ret;
	}

	public async download(name: string, arch: AppArch, version: AppVersion) {
		const { data } = await this.api.client.getBinary(name, version.raw, arch, { responseType: "arraybuffer" });
		const url = window.URL.createObjectURL(new Blob([data as ArrayBuffer]));
		const link = document.createElement("a");
		link.href = url;
		link.setAttribute("download", `${name}-${arch}-${version.raw}`); //or any other extension
		document.body.appendChild(link);
		link.click();
		document.body.removeChild(link);
	}

	public async upload(name: string, arch: AppArch, version: AppVersion, binary: any) {
		await this.api.client.add(name, version.raw, arch, btoa(binary));
	}
}
