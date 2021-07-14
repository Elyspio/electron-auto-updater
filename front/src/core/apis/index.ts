import {ExampleApi} from "./core"


export const Apis = {
    core: {
        example: new ExampleApi(undefined, window.config.endpoints.core)
    }
}




