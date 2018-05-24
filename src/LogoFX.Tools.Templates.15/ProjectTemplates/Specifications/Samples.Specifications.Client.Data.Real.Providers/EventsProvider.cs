using System;
using System.Collections.Generic;
using $saferootprojectname$.lient.Data.Contracts.Dto;
using $saferootprojectname$.lient.Data.Contracts.Providers;

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