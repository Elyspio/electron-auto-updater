import { AuthenticationApi } from "./authentication";

export const Apis = {
	authentication: new AuthenticationApi(undefined, "https://elyspio.fr/authentication"),
};
