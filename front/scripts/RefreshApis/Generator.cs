using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.TypeScript;
using NSwag;
using NSwag.CodeGeneration.OperationNameGenerators;
using NSwag.CodeGeneration.TypeScript;
using refresh_apis;

internal class Generator
{
    private readonly Dictionary<string, List<Schema>> alreadyGenerated = new();
    private readonly List<string> versions;
    private readonly string authFile;
    private string _generatePath;


    public Generator(List<string> versions, string authFile)
    {
        this.versions = versions;
        this.authFile = authFile;
        _generatePath = "";
    }

    public async Task GenerateTypeScriptClient(string url, string generatePath, string? version = null)
    {
        var document = await OpenApiDocument.FromUrlAsync(url);

        await GenerateClient(
            document,
            generatePath,
            document =>
            {
                var excludedParameterNames = new List<string>
                {
                    "x-token-claims-idTechPs",
                    "api-version"
                };
                var excludedSchemas = version != default ? GetExcludedSchemas(document) : new List<Schema>();

                var settings = new TypeScriptClientGeneratorSettings
                {
                    OperationNameGenerator = new MultipleClientsFromFirstTagAndOperationIdGenerator(),
                    Template = TypeScriptTemplate.Axios  ,
                    PromiseType = PromiseType.Promise,
                    UseAbortSignal = true,
                    GenerateOptionalParameters = true,
                    ExcludedParameterNames = excludedParameterNames.ToArray(),
                    ClassName = "{controller}Client",
                    GenerateClientInterfaces = true,
                    UseTransformOptionsMethod = false,
                    TypeScriptGeneratorSettings =
                    {
                        TemplateDirectory = Path.Combine(Environment.CurrentDirectory, "Templates"),
                        ExcludedTypeNames = excludedSchemas.Select<Schema, string>(schema => schema.name).ToArray(),
                        GenerateDefaultValues = true,
                        TypeStyle = TypeScriptTypeStyle.Interface,
                        EnumStyle = TypeScriptEnumStyle.Enum,
                        TypeScriptVersion = 4.7M,
                        DateTimeType = TypeScriptDateTimeType.String,
                        MarkOptionalProperties = true,
                        HandleReferences = true,
                        NullValue = TypeScriptNullValue.Undefined
                    }
                };

                var generator = new TypeScriptClientGenerator(document, settings);
                var code = generator.GenerateFile();

                if (version != default) alreadyGenerated[version] = GetSchemas(document, version);
                if (version != versions[0] && excludedSchemas.Any()) code = AddSchemaImport(code, excludedSchemas);

                return code;
            }
        );
    }

    private string AddSchemaImport(string code, List<Schema> schemas)
    {
        var generatedCode = code;
        
        var grouped = schemas.GroupBy(schema => schema.version);
        foreach (var group in grouped)
        {
            var names = group.Select(schema => schema.name).Where(cls => generatedCode.Contains(cls));
            var filename = $"{group.Key}";
            var imported = "{ " + string.Join(", ", names) + " }";
            code = $"import {imported} from \"./{filename}\"" + Environment.NewLine + code;
        }

        return code;
    }

    private async Task GenerateClient(OpenApiDocument document, string generatePath, Func<OpenApiDocument, string> generateCode)
    {

        _generatePath = generatePath;

        Console.WriteLine($"Generating {_generatePath} ...");

        var code = generateCode(document);

        await File.WriteAllTextAsync(_generatePath, code);
    }

    private List<Schema> GetSchemas(OpenApiDocument document, string? version = default)
    {
        return document.Components.Schemas.Select(pair =>
        {
            var (name, schema) = pair;
            return new Schema(name, schema.Discriminator, version);
        }).ToList();
    }

    private List<Schema> GetExcludedSchemas(OpenApiDocument document)
    {
        var schemas = GetSchemas(document);
        var alreadyGeneratedList = alreadyGenerated.SelectMany(x => x.Value);


        return alreadyGeneratedList.Where(schema => schemas.Any(s => s.ns == schema.ns)).ToList();
    }
}