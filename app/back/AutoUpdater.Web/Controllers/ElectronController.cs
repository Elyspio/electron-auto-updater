using AutoUpdater.Abstractions.Enums;
using AutoUpdater.Abstractions.Interfaces.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Net;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AutoUpdater.Web.Controllers;

[ApiController]
[Route("api/apps", Name = "ElectronApps")]
[Produces("application/json", "application/xml")]
public class ElectronController : ControllerBase
{
	private readonly ISerializer _serializer;
	private readonly IElectronService _electronService;

	public ElectronController(IElectronService electronService)
	{
		_electronService = electronService;
		_serializer = new SerializerBuilder()
			.WithNamingConvention(CamelCaseNamingConvention.Instance)
			.Build();
	}

	[HttpGet("{app}/{arch}/latest.yml")]
	[Produces("text/plain")]
	public async Task<string> GetLatestYml(string app, AppArch arch)
	{
		var yml = _serializer.Serialize(await _electronService.GetLatestYml(app, arch));
		return yml;
	}


	[HttpGet("{app}/{arch}/{version}.blockmap")]
	public async Task<byte[]> GetBlockmap(string app, AppArch arch, string version)
	{
		return await _electronService.GetBlockmap(app, arch, version);
	}


	[HttpPost("{app}/{arch}/{version}.blockmap")]
	[SwaggerResponse(HttpStatusCode.Created, typeof(void))]
	public async Task<IActionResult> AddBlockmap(string app, AppArch arch, string version, [FromBody] byte[] content)
	{
		 await _electronService.AddBlockmap(app, arch, version, content);

		return Created(Request.GetEncodedUrl(), null);
	}
}