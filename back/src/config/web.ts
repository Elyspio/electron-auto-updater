import * as path from "path";
import {$log} from "@tsed/common";


export const rootDir = path.resolve(__dirname, "..",);

let frontPath = process.env.FRONT_PATH ?? path.resolve(rootDir, "..", "..", "..", "front", "build")

$log.info({frontPath, rootDir});

export const webConfig: Partial<TsED.Configuration> = {
    rootDir,
    acceptMimes: ['application/json', 'text/plain'],
    httpPort: process.env.HTTP_PORT || 4000,
    httpsPort: false, // CHANGE
    mount: {
        '/core': [
            `${rootDir}/web/controllers/**/*.ts`
        ]
    },
    exclude: [
        '**/*.spec.ts'
    ],
    statics: {
        '/': [
            {root: frontPath,}
        ]
    },
    swagger: [{
        path: "/swagger",
        specVersion: "3.0.1"
    }]

};
