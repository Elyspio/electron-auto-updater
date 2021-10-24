import { Appender, BaseAppender, levels, LogEvent, Logger } from "@tsed/logger";
import * as gelfLog from "gelf-pro";
import { globalConf } from "../../config/global";
import { graylogConfig } from "../../config/logger";

const level = levels();

const levelMapping = {
	[level.DEBUG.levelStr]: 7,
	[level.INFO.levelStr]: 6,
	[level.WARN.levelStr]: 4,
	[level.ERROR.levelStr]: 3,
	[level.FATAL.levelStr]: 2,
};

gelfLog.setConfig(graylogConfig);

@Appender({ name: "graylog" })
export class ConsoleAppender extends BaseAppender {
	write(data: LogEvent) {
		const lvl = levelMapping[data.level.levelStr];

		const msg = data.data;

		let isFirstObject = typeof msg[0] === "object";
		const message = `${globalConf.appName} - ${data.categoryName} - ` + (isFirstObject ? `Complex data` : msg[0]);

		gelfLog.message(
			message,
			lvl,
			isFirstObject
				? msg[0]
				: {
						...msg[1],
						app: globalConf.appName,
						node: data.categoryName,
				  }
		);
	}
}

type LoggerName = string | Function | { constructor: { name: string } };

export function getLogger(name: LoggerName, type?: "Middleware" | "Service" | "Controller", autoMapping = false) {
	const nameType = typeof name;
	if (nameType === "object" && name.constructor?.name) {
		name = name.constructor.name;
	} else if (nameType !== "string") {
		name = (name as Function).name;
	}

	const log = new Logger(`${type ? `${type} - ` : ""}${name}`);
	log.appenders
		.set("console", {
			type: "console",
		})
		.set("std-log", {
			type: "graylog",
			level: ["debug", "info", "trace"],
		});

	return log;
}

getLogger.service = (name: LoggerName) => getLogger(name, "Service", true);
getLogger.middleware = (name: LoggerName) => getLogger(name, "Middleware", true);
getLogger.controller = (name: LoggerName) => getLogger(name, "Controller", true);
getLogger.default = () => getLogger("Default", undefined, true);
