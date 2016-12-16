using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Attest.Fake.Core;
using Attest.Fake.Setup.Contracts;
using LogoFX.Client.Data.Fake.ProviderBuilders;
using $saferootprojectname$.Client.Data.Contracts.Dto;
using $saferootprojectname$.Client.Data.Contracts.Providers;

namespace $safeprojectname$
{
    public sealed class EventsProviderBuilder : FakeBuilderBase<IEventsProvider>
    {
        private readonly List<EventDto> _events = new List<EventDto>();

        private Timer _timer;
        private readonly Random _rnd = new Random();

        private EventsProviderBuilder()
        {
            _timer = new Timer(OnTimer, null, 1000, 1000);
        }

        private void OnTimer(object state)
        {
            if (_rnd.NextDouble() < 0.7)
            {
                return;
            }

            WithEvent(string.Format("Sample Message #{0}", _rnd.Next(1, 100)));
        }

        public static EventsProviderBuilder CreateBuilder()
        {
            return new EventsProviderBuilder();
        }

        protected override IServiceCall<IEventsProvider> CreateServiceCall(IHaveNoMethods<IEventsProvider> serviceCallTemplate)
        {
            var setup = serviceCallTemplate
                .AddMethodCallWithResultAsync<DateTime, IEnumerable<EventDto>>(
                    t => t.GetLastEvents(It.IsAny<DateTime>()),
                    (r, lastEventTime) => r.Complete(_events.Where(x => x.Time >= lastEventTime)));
            return setup;
        }

        public void WithEvent(string message)
        {
            var evt = new EventDto
            {
                Time = DateTime.Now,
                Message = message
            };
            _events.Add(evt);
        }
    }
}