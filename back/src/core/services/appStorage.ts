import {AppDescription, Platform} from "../../controllers/appStorage/appStorage";
import * as path from "path";
import {applicationPath} from "../../config/appStorage";
import {ensureDir, access, lstat, readdir, readFile, writeFile} from "fs-extra";
import {ApplicationPlatform} from "../../controllers/types/request";

const metadata = "metadata.json"

type Metadata = {
    version: {
        windows: string[],
        linux: string[]
    }
}

export module Core.AppStorage {

    function getAppPath(app: string) {
        return path.join(applicationPath, app);
    }

    async function readMetadata(app: string): Promise<Metadata> {

        try {
            const json = (await readFile(path.join(getAppPath(app), metadata))).toString();
            return JSON.parse(json);
        }
        catch(e) {
            return {
                version: {
                    linux: [],
                    windows: []
                }
            }
        }


    }

    export async function storeApp(app: string, version: string, platform:  Platform, data: number[]) {
        const appPath = getAppPath(app);

        await ensureDir(appPath)
        await writeFile(path.join(appPath, version + (platform === "windows" ? ".exe" : "")), Buffer.from(data))

        const meta = await readMetadata(app);
        meta.version[platform].push(version);
        await writeFile(path.join(appPath, metadata), JSON.stringify(meta));
    }

    export async function getLatestVersion(app: string, platform: Platform) {
        const json = await readMetadata(app)
        return json.version[platform][json.version[platform].length - 1];
    }

    export async function getLatest(app: string, platform: "windows" | "linux") {
        const files = await readdir(getAppPath(app));

        const platformApps = files.filter(app => {
            if (platform === "windows") return app.slice(app.length - 4) === ".exe"
            if (platform === "linux") return app.slice(app.length - 4) !== ".exe"
        })

        const filesWithMtime = await Promise.all(platformApps.map(async f => {
            let filePath = path.join(getAppPath(app), f);
            return ({path: filePath, mtime: (await lstat(filePath)).mtimeMs});
        }));

        const latest = filesWithMtime.reduce((prev, current) => {
            if (current.mtime > prev.mtime) return current;
            return prev;
        })

        return  readFile(latest.path);
    }

    export async function getVersions(app: string) {
        const {version} = await readMetadata(app);
        return version;
    }


}
