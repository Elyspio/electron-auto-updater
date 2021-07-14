import {Apis} from "../apis";

export class AppStorageService {

    async listApps() {
        return Apis.core.appStorage.appStorageGetApps();
    }

}
