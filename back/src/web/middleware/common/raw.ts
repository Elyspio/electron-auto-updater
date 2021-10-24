import * as cookieParser from "cookie-parser";
import * as cors from "cors";
import * as compress from "compression";
import * as methodOverride from "method-override";
import * as express from "express";

export const middlewares: any[] = [];

middlewares.push(
	express.json({ limit: "1000mb" }),
	express.urlencoded({
		limit: "1000mb",
		extended: true,
	}),
	cors(),
	cookieParser(),
	compress({}),
	methodOverride()
);
