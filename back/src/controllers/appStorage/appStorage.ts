import {BodyParams, Controller, Get, PathParams, Post, Res} from '@tsed/common';
import {Core} from '../../core/services/appStorage';
import * as Express from 'express';

export type AppDescription = { version: string };
export type Platform = 'windows' | 'linux'

@Controller('/')

export class AppStorage {

    @Post('/:app/:platform')
    async addApp(
        @PathParams() {platform, app}: { app: string, platform: Platform },
        @BodyParams() {version, data}: { data: number[], version: string }) {
        await Core.AppStorage.storeApp(app, version, platform, data);
        return {status: "OK"};
    }

    @Get('/:app/version')
    async getVersions(@PathParams('app') app: string) {
        return Core.AppStorage.getVersions(app);

    }

    @Get('/:app/:platform/version')
    async getLatestVersion(@PathParams('app') app: string, @PathParams('platform') platform: Platform) {
        return Core.AppStorage.getLatestVersion(app, platform);
    }

    @Get('/:app/:platform')
    async getLatest(@PathParams('app') app: string, @PathParams('platform') platform: Platform, @Res() res: Express.Response) {
        const binary = await Core.AppStorage.getLatest(app, platform);
        const fileName = `${app}-installer${platform === "windows" ? ".exe" : ""}`
        const mime = "application/vnd.microsoft.portable-executable"
        res.writeHead(200, {
            'Content-Disposition': `attachment; filename="${fileName}"`,
            'Content-Type': mime,
            'Content-Length': binary.length
        })
        res.end(binary, 'binary');
    }


    @Get('/:app/:platform/:version')
    async getAppWithVersion(@PathParams('app') app: string, @PathParams('platform') platform: Platform, @PathParams("version") version: string, @Res() res: Express.Response) {
        const binary = await Core.AppStorage.getBinary(app, platform, version);
        const fileName = `${app}-${version}-installer${platform === "windows" ? ".exe" : ""}`
        const mime = "application/vnd.microsoft.portable-executable"
        res.writeHead(200, {
            'Content-Disposition': `attachment; filename="${fileName}"`,
            'Content-Type': mime,
            'Content-Length': binary.length
        })
        res.end(binary, 'binary');
    }


    @Get("/")
    async getApps() {
        return Core.AppStorage.getApps()
    }
}
