import { injectable } from "inversify";
import { AppsClient } from "./generated";
import axios from "axios";

const instance = axios.create({
	withCredentials: true,
	transformResponse: [],
});

@injectable()
export class AppsApiClient {
	public readonly client = new AppsClient(window.config.endpoints.core, instance);
}
