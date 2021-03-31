import {Apis} from "../apis";

export class CoreService {

    async listApps() {
        return Apis.core.appStorage.appStorageGetApps();
    }

}
