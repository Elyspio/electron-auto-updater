import {Api} from "./api";
import {endpoint} from "../config/appStorage";


export class AppStorageApi extends Api {

    private static _instance: AppStorageApi = new AppStorageApi(endpoint);

    public static get instance() {
        return this._instance;
    }

    public async getApps() {
        return await this.get<string[]>("/");
    }

    public async getAppVersions(app: string) {
        return await this.get<string[]>(`/${app}/version`);

    }


}
