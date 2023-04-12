using Microsoft.AspNetCore.Http;

namespace AutoUpdater.Abstractions.Extensions;

public static class StreamExtensions
{
	public async static Task<byte[]> ReadAllBytes(this IFormFile file)
	{
		using var stream = new MemoryStream();
		await file.CopyToAsync(stream);
		return stream.ToArray();
	}
}