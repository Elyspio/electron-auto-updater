﻿using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;
using System.Reflection;

namespace AutoUpdater.Core.Utils;

public class CallerEnricher : ILogEventEnricher
{
	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		var skip = 3;
		while (true)
		{
			var stack = new StackFrame(skip);
			if (!stack.HasMethod())
			{
				logEvent.AddPropertyIfAbsent(new("Caller", new ScalarValue("<unknown method>")));
				return;
			}

			var method = stack.GetMethod();
			if (method!.DeclaringType!.Assembly != typeof(Log).Assembly)
			{
				var caller = NeedLogging(method)
					? $" {method.DeclaringType.Name}.{method.Name}"
					: "";
				logEvent.AddPropertyIfAbsent(new("Caller", new ScalarValue(caller)));
				return;
			}

			skip++;
		}
	}

	private bool NeedLogging(MethodBase method)
	{
		return method!.DeclaringType!.FullName!.Contains("Backend");
	}
}

public static class LoggerCallerEnrichmentConfiguration
{
	public static LoggerConfiguration WithCaller(this LoggerEnrichmentConfiguration enrichmentConfiguration)
	{
		return enrichmentConfiguration.With<CallerEnricher>();
	}
}