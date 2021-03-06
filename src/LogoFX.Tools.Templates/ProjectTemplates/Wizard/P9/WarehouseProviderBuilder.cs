using System;
using System.Collections.Generic;
using System.Linq;
using Attest.Fake.Setup.Contracts;
using LogoFX.Client.Data.Fake.ProviderBuilders;
using $saferootprojectname$.Client.Data.Contracts.Dto;
using $saferootprojectname$.Client.Data.Contracts.Providers;
using Attest.Fake.Core;

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
                .AddMethodCallWithResult(t => t.GetWarehouseItems(),
                    r => r.Complete(GetWarehouseItems))
                .AddMethodCallWithResult<Guid, bool>(t => t.DeleteWarehouseItem(It.IsAny<Guid>()),
                    (r, id) => r.Complete(DeleteWarehouseItem(id)))
                .AddMethodCall<WarehouseItemDto>(t => t.SaveWarehouseItem(It.IsAny<WarehouseItemDto>()),
                    (r, dto) =>
                    {
                        SaveWarehouseItem(dto);
                        return r.Complete();
                    });
            return setup;
        }

        private IEnumerable<WarehouseItemDto> GetWarehouseItems()
        {
            return _warehouseItemsStorage;
        }

        private bool DeleteWarehouseItem(Guid id)
        {
            var dto = _warehouseItemsStorage.SingleOrDefault(x => x.Id == id);
            if (dto == null)
            {
                return false;
            }
            return _warehouseItemsStorage.Remove(dto);
        }

        private void SaveWarehouseItem(WarehouseItemDto dto)
        {
            var oldDto = _warehouseItemsStorage.SingleOrDefault(x => x.Id == dto.Id);
            if (oldDto == null)
            {
                _warehouseItemsStorage.Add(dto);
                return;
            }

            oldDto.Kind = dto.Kind;
            oldDto.Price = dto.Price;
            oldDto.Quantity = dto.Quantity;
        }
    }
}
