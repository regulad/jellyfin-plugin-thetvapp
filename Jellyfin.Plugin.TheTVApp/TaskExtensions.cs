using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Jellyfin.Plugin.TheTVApp;

/// <summary>
/// A collection of extensions for <see cref="Task"/> and related types.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Returns a unique identifier for the string that is deterministic and consistent.
    /// </summary>
    /// <param name="str">The string to generate an ID for.</param>
    /// <returns>The ID string.</returns>
    public static string IdOfString(this string str)
    {
        ArgumentNullException.ThrowIfNull(str);
        return unchecked((uint)str.GetHashCode(StringComparison.Ordinal)).ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Waits for all tasks to complete and returns only the successful results.
    /// Failed tasks trigger the error handler but their exceptions are not propagated.
    /// </summary>
    /// <typeparam name="T"> The type that the task returns. </typeparam>
    /// <param name="tasks"> The enumerable tasks that will all be awaited. </param>
    /// <param name="onError"> Optional error handler. </param>
    /// <returns> The successful results. </returns>
    public static async Task<IEnumerable<T>> WhenAllSuccessful<T>(
        IEnumerable<Task<T>> tasks,
        Action<Task<T>>? onError = null)
    {
        var taskArray = tasks.ToArray();

        // Create wrapper tasks that catch exceptions
        var wrappedTasks = taskArray.Select(async task =>
        {
            try
            {
                var result = await task.ConfigureAwait(false);
                return (Success: true, Result: result);
            }
            catch
            {
                onError?.Invoke(task);
                return (Success: false, Result: default(T));
            }
        });

        // Wait for all wrapped tasks
        var completedTasks = await Task.WhenAll(wrappedTasks).ConfigureAwait(false);

        // Filter successful results
        return completedTasks.Where(t => t.Success).Select(t => t.Result!);
    }
}
