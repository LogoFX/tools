using System;
using System.Threading;

namespace $safeprojectname$
{
    class RetryHelper
    {
        internal static TResult ExecuteWithRetry<TResult>(Func<TResult> action, int numberOfRetries, TimeSpan waitingInterval)            
        {
            for (int i = 0; i < numberOfRetries; i++)
            {
                try
                {
                    return action();                   
                }
                catch (Exception)
                {
                    Thread.Sleep(waitingInterval);
                }
            }
            return default(TResult);
        }
    }
}