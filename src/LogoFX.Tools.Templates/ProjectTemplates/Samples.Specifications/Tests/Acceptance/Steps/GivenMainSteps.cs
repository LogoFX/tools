#if FAKE
using System.Collections.Generic;
using LogoFX.Client.Testing.Contracts;
using Samples.Client.Data.Contracts.Dto;
using Samples.Specifications.Client.Data.Fake.ProviderBuilders;
#endif

#if REAL
#endif

namespace $safeprojectname$
{
    public class GivenMainSteps
    {
        private readonly IBuilderRegistrationService _builderRegistrationService;
        private readonly WarehouseProviderBuilder _warehouseProviderBuilder;

        public GivenMainSteps(
            IBuilderRegistrationService builderRegistrationService,
            WarehouseProviderBuilder warehouseProviderBuilder)
        {
            _builderRegistrationService = builderRegistrationService;
            _warehouseProviderBuilder = warehouseProviderBuilder;
        }

        public void SetupWarehouseItems(IEnumerable<WarehouseItemDto> warehouseItems)
        {
#if FAKE            
            _warehouseProviderBuilder.WithWarehouseItems(warehouseItems);
            _builderRegistrationService.RegisterBuilder(_warehouseProviderBuilder);
#endif

#if REAL
    //put here real Setup
#endif
        }
    }
}
