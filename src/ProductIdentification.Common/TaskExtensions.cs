using System;
using System.Threading.Tasks;

namespace ProductIdentification.Common
{
    public static class TaskExtensions
    {
        public static async void RunAndForget(
            this Task task, Action<Exception> onException = null)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                onException?.Invoke(ex);
            }
        }
    }
}