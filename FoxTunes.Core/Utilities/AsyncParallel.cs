﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoxTunes
{
    public static class AsyncParallel
    {
        public static Task For(int start, int count, Func<int, Task> factory, CancellationToken cancellationToken, ParallelOptions options)
        {
            return ForEach<int>(Enumerable.Range(start, count), factory, cancellationToken, options);
        }

        public static async Task ForEach<T>(IEnumerable<T> sequence, Func<T, Task> factory, CancellationToken cancellationToken, ParallelOptions options)
        {
            var exceptions = new List<Exception>();
            var tasks = new List<Task>(options.MaxDegreeOfParallelism);
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            foreach (var element in sequence)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
#if NET40
                tasks.Add(TaskEx.Run(async () =>
                {
                    try
                    {
                        await factory(element);
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(e);
                    }
                }));
#else
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        await factory(element);
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(e);
                    }
                }));
#endif
                if (exceptions.Count > 0)
                {
                    break;
                }
                if (tasks.Count == tasks.Capacity)
                {
#if NET40
                    await TaskEx.WhenAny(tasks);
#else
                    await Task.WhenAny(tasks);
#endif
                    tasks.RemoveAll(task => task.IsCompleted);
                }
            }
#if NET40
            await TaskEx.WhenAll(tasks);
#else
            await Task.WhenAll(tasks);
#endif
            if (exceptions.Any())
            {
                if (exceptions.Count == 1)
                {
                    throw exceptions.First();
                }
                throw new AggregateException(exceptions);
            }
        }

        public static async Task ForEach<T>(IEnumerable<T> sequence, Func<T, Task> factory, CancellationToken cancellationToken, IntegerConfigurationElement options)
        {
            var handler = new EventHandler((sender, e) => cancellationToken.Cancel());
            options.ValueChanged += handler;
            try
            {
                await ForEach<T>(sequence, factory, cancellationToken, new ParallelOptions()
                {
                    MaxDegreeOfParallelism = options.Value
                });
            }
            finally
            {
                options.ValueChanged -= handler;
            }
        }
    }
}
