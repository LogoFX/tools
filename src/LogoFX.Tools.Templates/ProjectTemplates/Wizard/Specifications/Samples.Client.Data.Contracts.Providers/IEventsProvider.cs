using System;
using System.Collections.Generic;
using $saferootprojectname$.Client.Data.Contracts.Dto;

namespace $safeprojectname$
{
    public interface IEventsProvider
    {
        IEnumerable<EventDto> GetLastEvents(DateTime lastEventTime);
    }
}