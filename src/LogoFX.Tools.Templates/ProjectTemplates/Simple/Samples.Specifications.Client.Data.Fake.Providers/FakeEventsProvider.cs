using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using $saferootprojectname$.Client.Data.Contracts.Dto;
using $saferootprojectname$.Client.Data.Contracts.Providers;

namespace $safeprojectname$
{
    [UsedImplicitly]
    internal sealed class FakeEventsProvider : IEventsProvider
    {
        private readonly object _sync = new object();

        private readonly List<EventDto> _events = new List<EventDto>();

        private Timer _timer;

        private readonly Random _rnd = new Random();

        public FakeEventsProvider()
        {
            _timer = new Timer(OnTimer, null, 1000, 1000);
        }

        private void OnTimer(object state)
        {
            lock (_sync)
            {
                if (_rnd.NextDouble() < 0.7)
                {
                    return;
                }

                var eventDto = GenerateEvent();
                _events.Add(eventDto);
            }
        }

        private EventDto GenerateEvent()
        {
            EventDto eventDto = new EventDto();
            eventDto.Time = DateTime.Now;
            eventDto.Message = string.Format("Sample Message #{0}", _rnd.Next(1, 100));

            return eventDto;
        }

        public async Task<IEnumerable<EventDto>> GetLastEvents(DateTime lastEventTime)
        {
            List<EventDto> result;

            lock (_sync)
            {
                result = _events.Where(x => x.Time > lastEventTime).ToList();
            }

            await Task.Delay(100);

            return result;
        }
    }
}