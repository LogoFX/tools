using AutoMapper;
using $saferootprojectname$.Client.Data.Contracts.Dto;
using $safeprojectname$.Contracts;

namespace $safeprojectname$.Mappers
{
    internal static class EventMapper
    {
        public static IEvent MapToEvent(EventDto eventDto)
        {
            return Mapper.Map<Event>(eventDto);
        }
    }
}