export const DiKeysService = {
	theme: Symbol.for("ThemeService"),
	localStorage: {
		settings: Symbol.for("LocalStorageService:elyspio-authentication-settings"),
		validation: Symbol.for("LocalStorageService:elyspio-authentication-validation"),
	},
};
