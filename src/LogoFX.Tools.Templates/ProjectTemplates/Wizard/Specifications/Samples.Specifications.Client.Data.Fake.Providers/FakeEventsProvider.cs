using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Attest.Fake.Builders;
using JetBrains.Annotations;
using $saferootprojectname$.Client.Data.Contracts.Dto;
using $saferootprojectname$.Client.Data.Contracts.Providers;
using $saferootprojectname$.Client.Data.Fake.ProviderBuilders;

namespace $safeprojectname$
{
    [UsedImplicitly]
    class FakeEventsProvider : FakeProviderBase<EventsProviderBuilder, IEventsProvider>, IEventsProvider
    {
        private readonly EventsProviderBuilder _eventsProviderBuilder;

        public FakeEventsProvider(EventsProviderBuilder eventsProviderBuilder)
        {
            _eventsProviderBuilder = eventsProviderBuilder;
        }

        async Task<IEnumerable<EventDto>> IEventsProvider.GetLastEvents(DateTime lastEventTime)
        {
            var service = GetService(() => _eventsProviderBuilder, b => b);
            return await service.GetLastEvents(lastEventTime);
        }
    }
}