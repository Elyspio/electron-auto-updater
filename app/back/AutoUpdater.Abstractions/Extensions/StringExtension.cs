namespace AutoUpdater.Abstractions.Extensions;

public static class StringExtension
{
	public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest)
	{
		first = list.ElementAtOrDefault(0)!;
		rest = list.Skip(1).ToList();
	}

	public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest)
	{
		first = list.ElementAtOrDefault(0)!;
		second = list.ElementAtOrDefault(1)!;
		rest = list.Skip(2).ToList();
	}
}