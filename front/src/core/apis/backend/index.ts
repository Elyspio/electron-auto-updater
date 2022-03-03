import { injectable } from "inversify";
import { AppsApi } from "./generated";
import axios from "axios";
import { BaseAPI } from "./generated/base";

const instance = axios.create({
	withCredentials: true,
});

export type Newable<T> = { new (...args: ConstructorParameters<typeof BaseAPI>): T };

function createApi<T extends BaseAPI>(cls: Newable<T>): T {
	return new cls(undefined, window.config.endpoints.core, instance);
}

@injectable()
export class AppsApiClient {
	public readonly client = createApi(AppsApi);
}
