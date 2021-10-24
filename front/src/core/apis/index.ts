import { AppStorageApi } from "./backend";

export const Apis = {
	core: {
		appStorage: new AppStorageApi(undefined, window.config.endpoints.core),
	},
};
