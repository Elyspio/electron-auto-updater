using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using System.Reflection;

namespace AutoUpdater.Web.Processors;

/// <summary>
///     Make all schema's properties required unless there are marked as optional (?)
/// </summary>
public class NullableOperationProcessor : IOperationProcessor
{
	private static readonly NullabilityInfoContext Context = new();

	/// <inheritdoc />
	public bool Process(OperationProcessorContext context)
	{
		context.OperationDescription.Operation.OperationId = context.MethodInfo.Name;

		foreach (var (param, prop) in context.Parameters)
		{
			var nullable = IsNullable(param);
			prop.IsRequired = !nullable;
			prop.Schema.IsNullableRaw = nullable;
		}

		return true;
	}


	private static bool IsNullable(ParameterInfo p)
	{
		var nullabilityInfo = Context.Create(p);
		return nullabilityInfo.WriteState is NullabilityState.Nullable;
	}
}