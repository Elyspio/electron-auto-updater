﻿using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoUpdater.Abstractions.Helpers;

public static class Log
{
	private static readonly JsonSerializerOptions options = new()
	{
		Converters =
		{
			new JsonStringEnumConverter()
		}
	};

	public static string F(object? value, [CallerArgumentExpression("value")] string name = "")
	{
		return $"{name}={JsonSerializer.Serialize(value, options)}";
	}


	public static LoggerInstance<T> Enter<T>(this ILogger<T> logger, string arguments = "", LogLevel level = LogLevel.Debug, [CallerMemberName] string method = "")
	{
		var loggerInstance = new LoggerInstance<T>(logger, method, arguments, level);


		return loggerInstance;
	}


	public class LoggerInstance<T>
	{
		private readonly string _arguments;
		private readonly LogLevel _level;
		private readonly ILogger<T> _logger;
		private readonly Guid _logId;
		private readonly string _method;
		private readonly DateTime _startedAt;

		public LoggerInstance(ILogger<T> logger, string method, string arguments, LogLevel level)
		{
			_arguments = arguments;
			_level = level;
			_method = method;
			_logger = logger;
			_startedAt = DateTime.Now;
			_logId = Guid.NewGuid();
			Enter();
		}

		private void Enter()
		{
			if (!_logger.IsEnabled(_level)) return;

			var sb = new StringBuilder($"- {_logId} - Entering method {_method}");
			if (_arguments?.Length > 0) sb.Append($" - {_arguments}");
			_logger.Log(_level, sb.ToString());
		}


		public void Log(string content, LogLevel level = LogLevel.Debug)
		{
			if (!_logger.IsEnabled(LogLevel.Information)) return;

			var sb = new StringBuilder($"- {_logId} - Logging method {_method}");
			sb.Append($" {content}");
			_logger.Log(level, sb.ToString());
		}

		public void Exit(string? content = null)
		{
			if (!_logger.IsEnabled(_level)) return;

			var sb = new StringBuilder($"- {_logId} - Exiting method {_method}");
			if (_arguments?.Length > 0) sb.Append($" - {_arguments}");
			if (!string.IsNullOrWhiteSpace(content)) sb.Append($" {content}");
			sb.Append($" ({(DateTime.Now - _startedAt).Milliseconds}ms)");
			_logger.Log(_level, sb.ToString());
		}
	}
}