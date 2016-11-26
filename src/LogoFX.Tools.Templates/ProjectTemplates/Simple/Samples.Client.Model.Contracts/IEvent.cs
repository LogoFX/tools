using System;

namespace $safeprojectname$
{
    public interface IEvent : IAppModel
    {
        DateTime Time { get; }
        string Message { get; }
    }
}