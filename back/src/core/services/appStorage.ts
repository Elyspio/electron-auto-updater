import {Platform} from "../../controllers/appStorage/appStorage";
import * as path from "path";
import {applicationPath} from "../../config/appStorage";
import {ensureDir, ensureDirSync, lstat, readdir, readFile, writeFile} from "fs-extra";

const metadata = "metadata.json"


export type Version = {
    val: string,
    date: Date
}


type Metadata = {
    version: {
        windows: Version[],
        linux: Version[]
    }
}


ensureDirSync(applicationPath);

export module Core.AppStorage {

    function getAppPath(app: string) {
        return path.join(applicationPath, app);
    }

    async function readMetadata(app: string): Promise<Metadata> {

        try {
            const json = (await readFile(path.join(getAppPath(app), metadata))).toString();
            return JSON.parse(json);
        } catch (e) {
            return {
                version: {
                    linux: [],
                    windows: []
                }
            }
        }


    }

    export async function storeApp(app: string, version: string, platform: Platform, data: number[]) {
        const appPath = getAppPath(app);
        await ensureDir(appPath);
        await writeFile(path.join(appPath, version + (platform === "windows" ? ".exe" : "")), Buffer.from(data))

        const meta = await readMetadata(app);
        meta.version[platform].push({val: version, date: new Date()});
        await writeFile(path.join(appPath, metadata), JSON.stringify(meta));
    }

    export async function getLatestVersion(app: string, platform: Platform) {
        const json = await readMetadata(app)
        return json.version[platform][json.version[platform].length - 1];
    }


    export async function getLatest(app: string, platform: string | Platform) {
        const files = await readdir(getAppPath(app));

        const platformApps = files
            .filter(x => x !== metadata)
            .filter(app =>
                platform === "windows"
                    ? app.endsWith(".exe")
                    : !app.endsWith(".exe")
            )

        const filesWithMtime = await Promise.all(platformApps.map(async f => {
            let filePath = path.join(getAppPath(app), f);
            return ({path: filePath, mtime: (await lstat(filePath)).mtimeMs});
        }));

        const latest = filesWithMtime.reduce((prev, current) => {
            if (current.mtime > prev.mtime) return current;
            return prev;
        })

        return readFile(latest.path);
    }

    export async function getVersions(app: string) {
        const {version} = await readMetadata(app);
        return version;
    }

    export async function getApps() {
        return await readdir(applicationPath);
    }

    export async function getBinary(app: string, platform: Platform, version: string) {
        const p = path.join(getAppPath(app), `${version}${platform === "windows" ? ".exe" : ""}`);
        return readFile(p);
    }


}
