import {Api} from "./api";
import {apiEndpoint} from "../../config/authentication";

export class AuthenticationService extends Api {

    public constructor() {
        super(apiEndpoint);
    }

    public isAuthenticated = async (token: string) => {
        const result = await this.post("/valid", {token});
        return result.status === 200;
    }

}
