using AutoUpdater.Abstractions.Enums;
using AutoUpdater.Abstractions.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
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
}