import * as path from "path";


export const rootDir = path.resolve(__dirname, "..",);

let frontPath = process.env.FRONT_PATH ?? path.resolve(rootDir, "..", "..", "..", "front", "build")

export const webConfig: Partial<TsED.Configuration> = {
	rootDir,
	acceptMimes: ['application/json', 'text/plain'],
	httpPort: process.env.HTTP_PORT || 4000,
	httpsPort: false, // CHANGE
	mount: {
		'/api': [
			`${rootDir}/web/controllers/**/*.ts`
		]
	},
	exclude: [
		'**/*.spec.ts'
	],
	statics: {
		'/': [
			{root: frontPath,}
		]
	},
	swagger: [{
		path: "/swagger",
		specVersion: "3.0.3"
	}]

};
