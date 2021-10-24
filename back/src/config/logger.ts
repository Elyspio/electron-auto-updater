import "../core/utils/logger";
import { Settings } from "gelf-pro";

export const graylogConfig: Partial<Settings> = {
	// this object is passed to the adapter.connect() method
	adapterOptions: {
		host: process.env.GRAYLOG_HOST ?? "192.168.0.59", // optional; default: 127.0.0.1
		port: process.env.GRAYLOG_PORT ? Number.parseInt(process.env.GRAYLOG_PORT) : 12201, // optional; default: 12201
	},
};
