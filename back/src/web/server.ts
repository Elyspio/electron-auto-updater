import {Configuration, Inject} from "@tsed/di";
import {PlatformApplication} from "@tsed/common";
import {middlewares} from "./middleware/common/raw";
import "@tsed/swagger";
import {webConfig} from "../config/web";


@Configuration(webConfig)
export class Server {

    @Inject()
    app: PlatformApplication;

    @Configuration()
    settings: Configuration;

    $beforeRoutesInit() {
        this.app.use(...middlewares)
        return null;
    }
}
