import { inject, injectable } from "inversify";
import { AppsApiClient } from "../apis/backend";
import { AppArch, AppVersion } from "../apis/backend/generated";
import { toast } from "react-toastify";
import { AppDownloadProgress } from "../../view/components/updater/AppDownloadProgress";
import { download } from "../utils/web";

@injectable()
export class AppsService {
	@inject(AppsApiClient)
	private api!: AppsApiClient;

	constructor() {
		console.log("Constructor");
	}

	public getAppsName() {
		return this.api.client.getApps();
	}

	public async getAppVersions(name: string) {
		const allMetadata = await this.api.client.getAllMetadata(name);

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
		const toastId = toast.loading(AppDownloadProgress({ state: "waiting", arch, version, name }));

		const { data } = await this.api.client.getBinary(name, version.raw, arch, undefined, {
			onDownloadProgress: progressEvent => {
				const percentage = progressEvent.loaded / progressEvent.total!;
				if (percentage >= 100) {
					toast.done(toastId);
				} else {
					toast.update(toastId, {
						render: AppDownloadProgress({ state: "downloading", arch, version, name }),
						progress: percentage,
					});
				}
			},
		});
		download(`${name} ${arch} ${version.raw}${this.getFileExtension(arch)}`, await data.arrayBuffer());
	}

	public async delete(name: string, arch: AppArch, version: AppVersion) {
		await this.api.client.delete(name, version.raw, arch);
	}

	private getFileExtension(arch: AppArch) {
		switch (arch) {
			case "Win32":
			case "Win64":
				return ".exe";
			case "LinuxDeb":
				return ".deb";
			case "LinuxSnap":
				return ".snap";
			case "LinuxRpm":
				return ".rpm";
		}
	}
}
