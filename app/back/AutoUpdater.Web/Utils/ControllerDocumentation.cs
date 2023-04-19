using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace AutoUpdater.Web.Utils;

/// <inheritdoc />
public class ControllerDocumentationConvention : IControllerModelConvention
{
	/// <inheritdoc />
	public void Apply(ControllerModel controller)
	{
		if (controller == null) return;

		foreach (var attribute in controller.Attributes)
			if (attribute.GetType() == typeof(RouteAttribute))
			{
				var routeAttribute = (RouteAttribute) attribute;
				if (!string.IsNullOrWhiteSpace(routeAttribute.Name)) controller.ControllerName = routeAttribute.Name;
			}
	}
}