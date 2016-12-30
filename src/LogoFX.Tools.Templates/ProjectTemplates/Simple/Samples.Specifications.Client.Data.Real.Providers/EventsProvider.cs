using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using $saferootprojectname$.Client.Data.Contracts.Dto;
using $saferootprojectname$.Client.Data.Contracts.Providers;

namespace $safeprojectname$
{
    class EventsProvider : IEventsProvider
    {
        public Task<IEnumerable<EventDto>> GetLastEvents(DateTime lastEventTime)
        {
            throw new NotImplementedException();
        }
    }
}