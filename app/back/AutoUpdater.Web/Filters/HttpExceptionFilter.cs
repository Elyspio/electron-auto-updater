using AutoUpdater.Abstractions.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AutoUpdater.Web.Filters;

/// <summary>
///     Allow solution's project to return specific HTTP status code with <see cref="HttpException" />
/// </summary>
public class HttpExceptionFilter : ExceptionFilterAttribute
{
	/// <inheritdoc />
	public override void OnException(ExceptionContext context)
	{
		if (context.Exception is HttpException ex)
		{
			context.Result = new JsonResult(ex.ToString())
			{
				StatusCode = (int) ex.Code,
				Value = ex
			};
			Console.Error.WriteLineAsync($"Error: {ex.Code} ({(int) ex.Code}) | {ex.Message} | {ex.StackTrace?.Trim()}");
		}

		;
		base.OnException(context);
	}
}