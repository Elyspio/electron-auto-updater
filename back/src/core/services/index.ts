import {AuthenticationService} from "./authentication";
import {Storage} from "./storage";

export const Services = {
    authentication: new AuthenticationService(),
    storage: new Storage()
}
