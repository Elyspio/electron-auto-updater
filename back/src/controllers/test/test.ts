import {Controller, Get, UseAuth} from "@tsed/common";
import {RequireLogin} from "../../middleware/authentication";

@Controller("/test")
export class Test {

    @Get("/")
    async get() {
        return "Content that does not require authentication"
    }


    @Get("/admin")
    @UseAuth(RequireLogin)
    async getAdmin() {
        return "Admin content"
    }
}
