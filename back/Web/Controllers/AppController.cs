using Abstractions.Enums;
using Abstractions.Interfaces.Services;
using Abstractions.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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

    [HttpPost("{name}/{arch}/{version}")]
    [RequestFormLimits(BufferBodyLengthLimit = long.MaxValue)]
    public Task Add([Required] string name, [Required] string version, [Required] AppArch arch, [Required]  byte[] content)
    {
        var app = new App { Metadata = new AppMetadata
        {
            Arch = arch,
            Name = name,
            Version = version
        }, 
        Binary = content};
        return service.Add(app);
    }

    [HttpGet]
    public Task<string[]> GetApps()
    {
        return service.GetApps();
    }

    [HttpDelete("{name}/{arch}/{version}")]
    public Task Delete([Required] string name, [Required] string version, [Required] AppArch arch)
    {
        return service.Delete(name, (AppVersion)version, arch);
    }

    [HttpGet("{name}")]
    public Task<List<AppMetadata>> GetAllMetadata([Required] string name)
    {
        return service.GetAllMetadata(name);
    }

    [HttpGet("{name}/{arch}/{version}")]
    public async Task<IResult> GetBinary([Required] string name, [Required] string version, [Required] AppArch arch)
    {
        var bytes = await service.GetBinary(name, (AppVersion)version, arch);

        return Results.File(bytes, fileDownloadName: $"{name}-{arch}-{version}");

    }

    [HttpGet("{name}/version")]
    public Task<Dictionary<AppArch, List<AppVersion>>> GetLatestVersions([Required] string name)
    {
        return service.GetLatestVersion(name);
    }

    [HttpGet("{name}/{arch}/version")]
    public Task<AppVersion> GetLatestArchSpecificVersion([Required] string name, [Required] AppArch arch)
    {
        return service.GetLatestVersion(name, arch);
    }

}