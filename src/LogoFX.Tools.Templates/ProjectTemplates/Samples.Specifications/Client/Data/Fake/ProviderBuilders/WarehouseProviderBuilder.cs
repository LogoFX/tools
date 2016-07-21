using System.Collections.Generic;
using Attest.Fake.Setup.Contracts;
using LogoFX.Client.Data.Fake.ProviderBuilders;
using Samples.Client.Data.Contracts.Dto;
using Samples.Client.Data.Contracts.Providers;

namespace $safeprojectname$
{    
    public class WarehouseProviderBuilder : FakeBuilderBase<IWarehouseProvider>
    {
        private readonly List<WarehouseItemDto> _warehouseItemsStorage = new List<WarehouseItemDto>();

        private WarehouseProviderBuilder()
        {
            
        }

        public static WarehouseProviderBuilder CreateBuilder()
        {
            return new WarehouseProviderBuilder();
        }

        public void WithWarehouseItems(IEnumerable<WarehouseItemDto> warehouseItems)
        {
            _warehouseItemsStorage.Clear();
            _warehouseItemsStorage.AddRange(warehouseItems);
        }        

        protected override IServiceCall<IWarehouseProvider> CreateServiceCall(IHaveNoMethods<IWarehouseProvider> serviceCallTemplate)
        {
            var setup = serviceCallTemplate
                .AddMethodCallWithResultAsync(t => t.GetWarehouseItems(),
                    r => r.Complete(GetWarehouseItems));
            return setup;
        }

        private IEnumerable<WarehouseItemDto> GetWarehouseItems()
        {
            return _warehouseItemsStorage;
        }
    }
}
