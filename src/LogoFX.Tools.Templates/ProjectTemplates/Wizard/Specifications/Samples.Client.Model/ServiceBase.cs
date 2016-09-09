using System;
using System.Threading.Tasks;
using Solid.Practices.Scheduling;

namespace $safeprojectname$
{
    abstract class ServiceBase
    {
        private readonly TaskFactory _taskFactory = TaskFactoryFactory.CreateTaskFactory();

        protected Task RunAsync(Action action)
        {
            return _taskFactory.StartNew(action);
        }

        protected Task<TResult> RunAsync<TResult>(Func<TResult> func)
        {
            return _taskFactory.StartNew(func);
        }
    }
}