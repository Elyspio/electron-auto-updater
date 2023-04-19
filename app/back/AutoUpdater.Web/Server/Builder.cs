using AutoUpdater.Abstractions.Interfaces.Injections;
using AutoUpdater.Core.Injections;
using AutoUpdater.Core.Utils;
using AutoUpdater.Db.Injections;
using AutoUpdater.Web.Filters;
using AutoUpdater.Web.Processors;
using AutoUpdater.Web.Utils;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Newtonsoft.Json.Converters;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Net;
using System.Text.Json.Serialization;

namespace AutoUpdater.Web.Server;

/// <summary>
///     Custom application builder
/// </summary>
public class ServerBuilder
{
	private readonly string _frontPath = Env.Get("FRONT_PATH", "/front");

	/// <param name="args">arguments from CLI</param>
	public ServerBuilder(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		builder.WebHost.ConfigureKestrel((_, options) =>
			{
				options.Listen(IPAddress.Any, 4000, listenOptions =>
					{
						// Use HTTP/3
						listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
					}
				);
			}
		);


		// Setup CORS
		if (builder.Environment.IsDevelopment())
			builder.Services.AddCors(options =>
				{
					options.AddDefaultPolicy(b =>
						{
							b.WithOrigins("http://localhost", "http://localhost:3000");
							b.AllowAnyHeader();
							b.AllowAnyMethod();
							b.AllowCredentials();
						}
					);
				}
			);


		builder.Services.AddModule<CoreModule>(builder.Configuration);
		builder.Services.AddModule<DatabaseModule>(builder.Configuration);

		// Setup Logging
		builder.Host.UseSerilog((_, lc) => lc
			.MinimumLevel.Debug()
			.Filter.ByExcluding(e => e.Level == LogEventLevel.Debug && e.Properties["SourceContext"].ToString().Contains("Microsoft"))
			.Enrich.FromLogContext()
			.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level} {SourceContext:l}] {Message:lj}{NewLine}{Exception}", theme: AnsiConsoleTheme.Sixteen)
		);

		// Convert Enum to String 
		builder.Services.AddControllers(o =>
				{
					o.RespectBrowserAcceptHeader = true;
					o.Conventions.Add(new ControllerDocumentationConvention());
					o.Filters.Add<HttpExceptionFilter>();
					o.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
				}
			)
			.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
			.AddNewtonsoftJson(x => x.SerializerSettings.Converters.Add(new StringEnumConverter()));

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddOpenApiDocument(document =>
		{
			document.Title = "App.Updater";

			document.SchemaProcessors.Add(new NullableSchemaProcessor());
			document.OperationProcessors.Add(new NullableOperationProcessor());
		});
		// Setup SPA Serving
		if (builder.Environment.IsProduction()) Console.WriteLine($"Server in production, serving SPA from {_frontPath} folder");

		Application = builder.Build();
	}

	/// <summary>
	///     Built application
	/// </summary>
	public WebApplication Application { get; }
}