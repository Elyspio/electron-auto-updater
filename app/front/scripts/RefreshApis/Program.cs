
var path = Path.Combine(Environment.CurrentDirectory, "..", "..", "src", "core", "apis", "backend");

if (!File.Exists(path))
{
    Directory.CreateDirectory(path);
};

path = Path.GetFullPath(path);

var versions = new List<string>()
{
	"v1",
};

var generator = new Generator(versions, Path.Combine(path, "authorization"));
foreach (var version in versions)
{
	await generator.GenerateTypeScriptClient(
		$"http://localhost:4000/swagger/{version}/swagger.json",
		Path.Combine(path, $"generated.ts"),
		version
	);
}


