using System.Collections.Generic;
using $saferootprojectname$.Client.Data.Contracts.Dto;

#if FAKE
using LogoFX.Client.Testing.Contracts;
using $saferootprojectname$.Client.Data.Fake.ProviderBuilders;
#endif

#if REAL

#endif

namespace $safeprojectname$
{
    public class GivenMainSteps
    {
#if FAKE
        private readonly IBuilderRegistrationService _builderRegistrationService;
        private readonly WarehouseProviderBuilder _warehouseProviderBuilder;

        public GivenMainSteps(
            IBuilderRegistrationService builderRegistrationService,
            WarehouseProviderBuilder warehouseProviderBuilder)
        {
            _builderRegistrationService = builderRegistrationService;
            _warehouseProviderBuilder = warehouseProviderBuilder;
        }
#endif
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
