using System;
using System.Collections.Generic;
using Attest.Fake.Builders;
using JetBrains.Annotations;
using $saferootprojectname$.lient.Data.Contracts.Dto;
using $saferootprojectname$.lient.Data.Contracts.Providers;
using $saferootprojectname$.pecifications.Client.Data.Fake.ProviderBuilders;

namespace $safeprojectname$
{
    [UsedImplicitly]
    class FakeEventsProvider : FakeProviderBase<EventsProviderBuilder, IEventsProvider>, IEventsProvider
    {
        public FakeEventsProvider(EventsProviderBuilder eventsProviderBuilder)
            :base(eventsProviderBuilder)
        {
        }

        IEnumerable<EventDto> IEventsProvider.GetLastEvents(DateTime lastEventTime)
        {
            var service = GetService();
            return service.GetLastEvents(lastEventTime);
        }
    }
}