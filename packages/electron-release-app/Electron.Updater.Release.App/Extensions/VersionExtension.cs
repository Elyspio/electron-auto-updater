// ReSharper disable once CheckNamespace

namespace SwaggerMerge.ReleaseApp.UpdaterClient;

public partial class AppVersion
{
	public static implicit operator AppVersion(string raw)
	{
		var split = raw.Split(".");
		return new()
		{
			Raw = raw,
			Major = int.Parse(split[0]),
			Minor = int.Parse(split[1]),
			Revision = int.Parse(split[2])
		};
	}

	public AppVersion Clone()
	{
		return Raw;
	}

	public override string ToString()
	{
		Raw = $"{Major}.{Minor}.{Revision}";
		return Raw;
	}
}