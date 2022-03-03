import { Container } from "inversify";

export const container = new Container({ defaultScope: "Singleton" });

require("./apis/di.api");
require("./services/di.service");
