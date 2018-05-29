using System;
using $safeprojectname$.Contracts;

namespace $safeprojectname$
{
    internal sealed class Event : AppModel, IEvent
    {
        public DateTime Time { get; set; }
        public string Message { get; set; }
    }
}