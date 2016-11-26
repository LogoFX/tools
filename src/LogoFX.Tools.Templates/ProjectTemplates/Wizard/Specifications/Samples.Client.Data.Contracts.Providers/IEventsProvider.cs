using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using $saferootprojectname$.Client.Data.Contracts.Dto;

namespace $safeprojectname$
{
    public interface IEventsProvider
    {
        Task<IEnumerable<EventDto>> GetLastEvents(DateTime lastEventTime);
    }
}