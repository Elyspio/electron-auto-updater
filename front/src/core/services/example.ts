import {Apis} from "../apis";

export class ExampleService {
    getContent() {
        return Apis.core.example.exampleGet()
    }

    async getAdminContent() {
        try {
            const conf = {query: {token: document.cookie.slice(document.cookie.indexOf("=") + 1)}}
            const a = await Apis.core.example.exampleGetAdmin(conf)
            return a.data
        } catch (e) {
            // Api.redirect(getLoginPage())
        }
    }
}
