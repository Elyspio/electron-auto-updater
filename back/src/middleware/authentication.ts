import {BodyParams, IMiddleware, Middleware, Req} from "@tsed/common";
import {Unauthorized} from "@tsed/exceptions";
import {AuthenticationService} from "../core/services/authentication";
import * as Express from "express"
import {authentication_token} from "../config/authentication";

@Middleware()
export class RequireLogin implements IMiddleware {
    public use(@Req() request: Express.Request, @BodyParams("token") token: string) {

        const tok = token ?? request.cookies[authentication_token]

        if (!new AuthenticationService().isAuthenticated(tok)) {
            throw new Unauthorized("You must be logged in to access to this page")
        }
    }
}
