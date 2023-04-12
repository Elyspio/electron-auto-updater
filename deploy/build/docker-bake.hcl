target "default" {
	context    = "../.."
	dockerfile = "./deploy/build/dockerfile"
	platforms  = [
		"linux/amd64",
#		"linux/arm64"
	]
	tags = [
		"elyspio/electron-auto-updater:latest"
	]
	args = {
		SLN_PATH         = "back/AutoUpdater.sln"
		MAIN_CSPROJ_PATH = "AutoUpdater.Web/AutoUpdater.Web.csproj"
		ROOT_FOLDER      = "back/"
		ENTRY_DLL        = "AutoUpdater.Web.dll"
	}
}