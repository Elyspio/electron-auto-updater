import { BodyParams, Controller, Get, PathParams, Post, QueryParams, Req, Res } from "@tsed/common";
import { Returns } from "@tsed/schema";
import * as Express from "express";
import { VersionFormatModel, VersionModel } from "./model";
import { Services } from "../../../core/services";

export type Platform = "windows" | "linux";

@Controller("/")
export class AppStorage {
	@Post("/:app/:platform")
	async addApp(
		@Req() req: Express.Request,
		@PathParams() { platform, app }: { app: string; platform: Platform },
		@BodyParams() { version, data }: { data: number[]; version: string }
	) {
		req.setTimeout(300 * 1e3);
		await Services.appStorage.storeApp(app, version, platform, data);
		return { status: "OK" };
	}

	@Get("/:app/version")
	async getVersions(@PathParams("app") app: string) {
		return Services.appStorage.getVersions(app);
	}

	@Get("/:app/:platform/version")
	@(Returns(200, String).ContentType("text/plain"))
	@(Returns(200, VersionModel).ContentType("application/json"))
	async getLatestVersion(@PathParams("app") app: string, @PathParams("platform") platform: Platform, @QueryParams(VersionFormatModel) { format }: VersionFormatModel) {
		const latestVersion = await Services.appStorage.getLatestVersion(app, platform);

		if (format === "string") return latestVersion.val;

		return latestVersion;
	}

	@Get("/:app/:platform")
	async getLatest(@PathParams("app") app: string, @PathParams("platform") platform: Platform, @Res() res: Express.Response) {
		const binary = await Services.appStorage.getLatest(app, platform);
		const fileName = `${app}-installer${platform === "windows" ? ".exe" : ""}`;
		const mime = "application/vnd.microsoft.portable-executable";
		res.writeHead(200, {
			"Content-Disposition": `attachment; filename="${fileName}"`,
			"Content-Type": mime,
			"Content-Length": binary.length,
		});
		res.end(binary, "binary");
	}

	@Get("/:app/:platform/:version")
	async getAppWithVersion(@PathParams("app") app: string, @PathParams("platform") platform: Platform, @PathParams("version") version: string, @Res() res: Express.Response) {
		const binary = await Services.appStorage.getBinary(app, platform, version);
		const fileName = `${app}-${version}-installer${platform === "windows" ? ".exe" : ""}`;
		const mime = "application/vnd.microsoft.portable-executable";
		res.writeHead(200, {
			"Content-Disposition": `attachment; filename="${fileName}"`,
			"Content-Type": mime,
			"Content-Length": binary.length,
		});
		res.end(binary, "binary");
	}

	@(Returns(200, Array).Of(String))
	@Get("/")
	async getApps() {
		return Services.appStorage.getApps();
	}
}
