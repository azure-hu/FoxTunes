using System.Threading.Tasks;

namespace FoxTunes
{
    public static class TaskHelper
    {
        public static Task CompletedTask
        {
            get
            {
                return FromResult<bool>(false);
            }
        }

        public static Task<T> FromResult<T>(T result)
        {
#if NET45
            return Task.FromResult<T>(result);
#elif NET40
            var source = new TaskCompletionSource<T>();
            source.SetResult(result);
            return source.Task;
#endif
        }
    }
}
