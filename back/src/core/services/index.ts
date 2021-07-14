import {AuthenticationService} from "./authentication";
import {Storage} from "./storage";
import {AppStorageService} from "./appStorage";

export const Services = {
	authentication: new AuthenticationService(),
	storage: new Storage(),
	appStorage: new AppStorageService()
}
