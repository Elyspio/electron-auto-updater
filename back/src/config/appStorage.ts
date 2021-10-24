import * as path from "path";

export const applicationPath = process.env.APPLICATIONS_LOCATION ?? path.resolve(__dirname, "..", "..", "apps");
