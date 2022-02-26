using Abstractions.Enums;
using Abstractions.Interfaces.Services;
using Abstractions.Models;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

[ApiController]
[Route("api/apps", Name = "Apps")]
public class AppController : ControllerBase
{

    private readonly IAppService service;

    public AppController(IAppService stackService)
    {
        this.service = stackService;
    }

    [HttpPost]
    [RequestFormLimits(BufferBodyLengthLimit = long.MaxValue)]
    public Task Add(AddApp model)
    {
        var app = new App { Metadata = model.Metadata, Binary = model.Binary.Select(b => (byte) b).ToArray()};
        return service.Add(app);
    }

    [HttpGet]
    public Task<string[]> GetApps()
    {
        return service.GetApps();
    }

    [HttpDelete("{name}/{arch}/{version}")]
    public Task Delete(string name, string version, AppArch arch)
    {
        return service.Delete(name, (AppVersion)version, arch);
    }

    [HttpGet("{name}")]
    public Task<List<AppMetadata>> GetAllMetadata(string name)
    {
        return service.GetAllMetadata(name);
    }

    [HttpGet("{name}/{arch}/{version}")]
    public async Task<IResult> GetBinary(string name, string version, AppArch arch)
    {
        var bytes = await service.GetBinary(name, (AppVersion)version, arch);

        return Results.File(bytes, fileDownloadName: $"{name}-{arch}-{version}");

    }

    [HttpGet("{name}/version")]
    public Task<Dictionary<AppArch, List<AppVersion>>> GetLatestVersions(string name)
    {
        return service.GetLatestVersion(name);
    }

    [HttpGet("{name}/{arch}/version")]
    public Task<AppVersion> GetLatestArchSpecificVersion(string name, AppArch arch)
    {
        return service.GetLatestVersion(name, arch);
    }

}