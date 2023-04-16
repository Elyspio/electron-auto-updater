using AutoUpdater.Abstractions.Enums;
using AutoUpdater.Abstractions.Extensions;
using AutoUpdater.Abstractions.Interfaces.Services;
using AutoUpdater.Abstractions.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AutoUpdater.Web.Controllers;

/// <summary>
///     Manage stored applications
/// </summary>
[ApiController]
[Route("api/apps", Name = "Apps")]
public class AppController : ControllerBase
{
	private readonly IAppService _service;

	/// <summary>
	///     AppController's constructor
	/// </summary>
	/// <param name="stackService"></param>
	public AppController(IAppService stackService)
	{
		_service = stackService;
	}

	/// <summary>
	///     Returns all apps app
	/// </summary>
	/// <returns></returns>
	[HttpGet("")]
	public Task<string[]> GetApps()
	{
		return _service.GetApps();
	}

	/// <summary>
	///     Add a new version for this application
	/// </summary>
	/// <param name="app"></param>
	/// <param name="version"></param>
	/// <param name="arch"></param>
	/// <param name="content"></param>
	[HttpPost("{app}/{arch}/{version}")]
	[RequestFormLimits(BufferBodyLengthLimit = long.MaxValue)]
	public async Task Add([Required] string app, [Required] string version, [Required] AppArch arch, [Required] IFormFile content)
	{
		await _service.Add(new()
		{
			Metadata = new()
			{
				Arch = arch,
				Version = version,
				Name = app
			},
			Binary = await content.ReadAllBytes()
		});
	}

	/// <summary>
	///     Add a new version for this application from bytes
	/// </summary>
	/// <param name="app"></param>
	/// <param name="version"></param>
	/// <param name="arch"></param>
	/// <param name="content"></param>
	[HttpPost("{app}/{arch}/{version}/bytes")]
	[RequestSizeLimit(long.MaxValue)]
	public async Task AddFromBytes([Required] string app, [Required] string version, [Required] AppArch arch, [FromBody] [Required] byte[] content)
	{
		await _service.Add(new()
		{
			Metadata = new()
			{
				Arch = arch,
				Version = version,
				Name = app
			},
			Binary = content
		});
	}


	/// <summary>
	///     Delete a specific version for this application
	/// </summary>
	/// <param name="app"></param>
	/// <param name="version"></param>
	/// <param name="arch"></param>
	/// <returns></returns>
	[HttpDelete("{app}/{arch}/{version}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public Task Delete([Required] string app, [Required] string version, [Required] AppArch arch)
	{
		return _service.Delete(app, version, arch);
	}

	/// <summary>
	///     Get binary for this app/arch/version
	/// </summary>
	/// <param name="app"></param>
	/// <param name="version"></param>
	/// <param name="arch"></param>
	/// <returns></returns>
	[HttpGet("{app}/{arch}/{version}")]
	public async Task<IResult> GetBinary([Required] string app, [Required] string version, [Required] AppArch arch)
	{
		var bytes = await _service.GetBinary(app, version, arch);

		var filename = $"{app}-{arch}-{version}";

		if (arch is AppArch.Win32 or AppArch.Win64) filename += ".exe";

		return Results.File(bytes, fileDownloadName: filename);
	}

	/// <summary>
	///     Get all versions for a specific arch
	/// </summary>
	/// <param name="app"></param>
	/// <returns></returns>
	[HttpGet("{app}/versions")]
	public Task<Dictionary<AppArch, AppVersion>> GetLatestVersions([Required] string app)
	{
		return _service.GetLatestArchSpecificVersion(app);
	}

	/// <summary>
	///     Get all versions for all arch for
	/// </summary>
	/// <param name="app"></param>
	/// <returns></returns>
	[HttpGet("{app}")]
	public Task<List<AppMetadata>> GetAllMetadata([Required] string app)
	{
		return _service.GetAllMetadata(app);
	}

	/// <summary>
	///     Get the latest version for this app
	/// </summary>
	/// <param name="app"></param>
	/// <returns></returns>
	[HttpGet("{app}/versions/latest")]
	public Task<AppVersion> GetLatestVersion(string app)
	{
		return _service.GetLatestVersion(app);
	}

	/// <summary>
	///     Get latest version for this arch
	/// </summary>
	/// <param name="app"></param>
	/// <param name="arch"></param>
	/// <returns></returns>
	[HttpGet("{app}/{arch}/version")]
	public Task<AppVersion> GetLatestArchSpecificVersion([Required] string app, [Required] AppArch arch)
	{
		return _service.GetLatestArchSpecificVersion(app, arch);
	}
}