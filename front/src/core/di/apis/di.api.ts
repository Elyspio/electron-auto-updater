import { DiKeysApi } from "./di.keys.api";
import { AppsApiClient } from "../../apis/backend";
import { container } from "../index";

container.bind<AppsApiClient>(DiKeysApi.apps).to(AppsApiClient);
container.bind<AppsApiClient>(AppsApiClient).toSelf();
