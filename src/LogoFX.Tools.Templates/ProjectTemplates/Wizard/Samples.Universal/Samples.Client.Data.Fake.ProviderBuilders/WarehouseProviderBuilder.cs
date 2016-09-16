using System.Collections.Generic;
using System.Threading.Tasks;
using Attest.Fake.Builders;
using Attest.Fake.LightMock;
using Attest.Fake.Setup;
using LightMock;
using $saferootprojectname$.Client.Data.Contracts.Dto;
using $saferootprojectname$.Client.Data.Contracts.Providers;

namespace $safeprojectname$
{            
    public class WarehouseProviderBuilder : FakeBuilderBase<IWarehouseProvider>
    {
        class WarehouseProviderProxy : ProviderProxyBase<IWarehouseProvider>, IWarehouseProvider
        {
            public WarehouseProviderProxy(IInvocationContext<IWarehouseProvider> context)
                : base(context)
            {
            }

            public Task<IEnumerable<WarehouseItemDto>> GetWarehouseItems()
            {
                return Invoke(t => t.GetWarehouseItems());
            }
        }

        private readonly List<WarehouseItemDto> _warehouseItemsStorage = new List<WarehouseItemDto>();

        private WarehouseProviderBuilder() :
            base(FakeFactoryHelper.CreateFake<IWarehouseProvider>(c => new WarehouseProviderProxy(c)))
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

        protected override void SetupFake()
        {
            var initialSetup = ServiceCallFactory.CreateServiceCall(FakeService);

            var setup = initialSetup
                .AddMethodCallWithResultAsync(t => t.GetWarehouseItems(),
                    r => r.Complete(GetWarehouseItems)); 
           
            setup.Build();
        }

        private IEnumerable<WarehouseItemDto> GetWarehouseItems()
        {
            return _warehouseItemsStorage;
        }
    }
}
