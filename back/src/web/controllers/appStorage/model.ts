import { Enum, Property, Required } from "@tsed/schema";
import { Version } from "../../../core/services/appStorage";

export class VersionModel implements Version {
	@Property()
	@Required()
	date: Date;

	@Property()
	@Required()
	val: string;
}

export class VersionFormatModel {
	@Enum("string")
	format?: "string";
}
