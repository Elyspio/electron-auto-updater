import {Apis} from "../apis";
import {$log} from "@tsed/common";

export class AuthenticationService {

    public isAuthenticated = async (token: string) => {
        const result = await Apis.authentication.authenticationValidToken({token});
        $log.info("AuthenticationService.isAuthenticated", {data: result.data, status: result.status})
        return result.status === 204;
    }

}
