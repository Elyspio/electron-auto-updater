import {Api} from "./api";
import {apiEndpoint} from "../config/authentication";


export class TestApi extends Api {

    private static _instance: TestApi = new TestApi(apiEndpoint);

    public static get instance() {
        return this._instance;
    }

    public async getContent() {
        return await this.get<string>("/");
    }

    public async getAdminContent() {
        const c = await this.get<string>("/");
        if (c.status === 200) {
            return c;
        } else
            return false;
    }
}
