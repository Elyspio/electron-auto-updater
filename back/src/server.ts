import {Configuration, Inject} from "@tsed/di";
import {$log, PlatformApplication} from "@tsed/common";
import {middlewares} from "./middleware/common/raw";
import * as path from "path";
import "@tsed/swagger"; // import swagger Ts.ED module
import "@tsed/seq"; // import swagger Ts.ED module

export const rootDir = __dirname;
let frontPath = process.env.NODE_ENV !== "production"
    ? path.resolve(rootDir, "..", "..", "front", "build")
    : "/app/front/build"


$log.name = process.env.APP_NAME ?? "Automatize -- Agent builder"

$log.appenders.set("file", {
    type: 'file',
    filename: path.resolve((process.env.LOG_FOLDER ?? `${__dirname}/../../logs`), "app.log"),
    pattern: '.yyyy-MM-dd',
    maxLogSize: 10485760,
    backups: 3,
    compress: true
});


$log.appenders.set("seq", {
    type: "seq",
    level: ["info", "error", "debug"],
    options: {
        serverUrl: 'https://elyspio.fr/logger/',
    }
});


@Configuration({
    rootDir,
    acceptMimes: ["application/json", "text/plain"],
    httpPort: process.env.HTTP_PORT || 4000,
    httpsPort: false, // CHANGE
    mount: {
        "/core": [
            `${rootDir}/controllers/**/*.ts`
        ]
    },
    exclude: [
        "**/*.spec.ts"
    ],
    statics: {
        "/": [
            {root: frontPath}
        ]
    },
    swagger: [{
        path: "/swagger",
        specVersion: "3.0.1"

    }],
    seq: {
        url: 'https://elyspio.fr/logger/',
    },

})
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

$log.info("INFO");
$log.warn("WARN");
$log.error("ERROR");
