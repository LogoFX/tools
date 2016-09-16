using System;
using System.Linq;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;

namespace $safeprojectname$
{
    public static class ApplicationExtensions
    {
        public static Window GetWindowEx(this Application app, string title)
        {
            app.WaitWhileBusy();
            return RetryHelper.ExecuteWithRetry(() => app.GetWindows().SingleOrDefault(x => x.Title == title), 3,
                TimeSpan.FromSeconds(5));            
        }
    }
}