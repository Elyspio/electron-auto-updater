import * as  bodyParser from "body-parser";
import * as cookieParser from 'cookie-parser';
import * as cors from "cors";
import * as compress from "compression";
import * as methodOverride from "method-override";

export const middlewares: any[] = [];


middlewares.push(
    cors(),
    cookieParser(),
    compress({}),
    methodOverride(),
    bodyParser.json({limit: "250mb"}),
    bodyParser.urlencoded({
        extended: true,
        limit: "250mb"
    }),
)

