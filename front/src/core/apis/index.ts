import {AppStorageApi} from "./back"
import process from "process";


function getEndpoint(env: string, fallback: string) {
    return process.env[env] ?? fallback;
}


export const Apis = {
    core: {
        appStorage: new AppStorageApi(undefined, getEndpoint("CORE_ENDPOINT", "http://localhost:4000/core"))
    }
}
