using System;
using System.Collections.Generic;
using $saferootprojectname$.Client.Data.Contracts.Dto;
using $saferootprojectname$.Client.Data.Contracts.Providers;

namespace $safeprojectname$
{
    class EventsProvider : IEventsProvider
    {
        public IEnumerable<EventDto> GetLastEvents(DateTime lastEventTime)
        {
            throw new NotImplementedException();
        }
    }
}