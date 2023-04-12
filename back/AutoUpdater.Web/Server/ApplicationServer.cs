namespace AutoUpdater.Web.Server;

/// <summary>
///     Application extension with specific calls for this app
/// </summary>
public static class ApplicationServer
{
	/// <summary>
	///     Initialize a custom built <see cref="ServerBuilder.Application" />
	/// </summary>
	/// <param name="application"></param>
	/// <returns></returns>
	public static WebApplication Initialize(this WebApplication application)
	{
		// Allow CORS

		if (application.Environment.IsDevelopment())
		{
			application.UseCors();
		}

		application.UseOpenApi(settings =>
		{

			if (!application.Environment.IsProduction()) return;

			settings.PostProcess = (document, request) =>
			{
				foreach (var server in document.Servers)
					if (!server.Url.Contains("https"))
						server.Url = server.Url.Replace("http", "https");
			};
		});
		application.UseSwaggerUi3();

		// Start Dependency Injection
		application.UseAdvancedDependencyInjection();

		// Setup Controllers
		application.MapControllers();

		application.UseAuthentication();

		if (!application.Environment.IsProduction()) return application;


		// Specific to production
		application.UseRouting();

		application.UseDefaultFiles(new DefaultFilesOptions
			{
				DefaultFileNames = new List<string>
				{
					"index.html"
				},
				RedirectToAppendTrailingSlash = true
			}
		);
		application.UseStaticFiles();

		application.MapFallbackToFile("/index.html");

		return application;
	}
}