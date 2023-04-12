using System.Collections.Concurrent;

namespace AutoUpdater.Abstractions.Extensions;

public static class ListExtension
{
	public async static Task<ParallelResult<TInput, TRet>> Parallelize<TInput, TRet>(this IEnumerable<TInput> elements, Func<TInput, Task<TRet>> action) where TInput : notnull
	{
		var innerExceptions = new ConcurrentDictionary<TInput, Exception>();

		var results = new ConcurrentBag<TRet>();


		await Parallel.ForEachAsync(elements, async (input, _) =>
		{
			try
			{
				var result = await action(input);
				results.Add(result);
			}
			catch (Exception e)
			{
				innerExceptions[input] = e;
			}
		});

		return new()
		{
			Data = results.ToList(),
			Exceptions = innerExceptions.ToDictionary(pair => pair.Key, pair => pair.Value),
			Status = innerExceptions.Any() ? ParallelStatus.Faulted : ParallelStatus.Succeed
		};
	}


	public async static Task<ParallelResult<TInput>> Parallelize<TInput>(this IEnumerable<TInput> elements, Func<TInput, Task> action) where TInput : notnull
	{
		var innerExceptions = new ConcurrentDictionary<TInput, Exception>();


		await Parallel.ForEachAsync(elements, async (input, _) =>
		{
			try
			{
				await action(input);
			}
			catch (Exception e)
			{
				innerExceptions[input] = e;
			}
		});

		return new()
		{
			Exceptions = innerExceptions.ToDictionary(pair => pair.Key, pair => pair.Value),
			Status = innerExceptions.Any() ? ParallelStatus.Faulted : ParallelStatus.Succeed
		};
	}

	public class ParallelResult<TInput, TRet> where TInput : notnull
	{
		public required List<TRet> Data { get; set; }
		public required Dictionary<TInput, Exception> Exceptions { get; set; }
		public ParallelStatus Status { get; set; }
	}


	public class ParallelResult<TInput> where TInput : notnull
	{
		public required Dictionary<TInput, Exception> Exceptions { get; set; }
		public ParallelStatus Status { get; set; }
	}
}

public enum ParallelStatus
{
	Succeed,
	Faulted
}