import { AppsApiClient } from "../../apis/backend";
import { container } from "../index";

container.bind<AppsApiClient>(AppsApiClient).toSelf();
