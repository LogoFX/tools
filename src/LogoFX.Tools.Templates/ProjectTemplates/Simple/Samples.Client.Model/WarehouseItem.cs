using System;
using JetBrains.Annotations;
using $safeprojectname$.Contracts;
using $safeprojectname$.Shared.Validation;

namespace $safeprojectname$
{
    [UsedImplicitly]
    internal sealed class WarehouseItem : AppModel, IWarehouseItem
    {
        public WarehouseItem(
            string kind, 
            double price, 
            int quantity)
        {
            Id = Guid.NewGuid();
            _kind = kind;
            _price = price;
            _quantity = quantity;            
        }

        private string _kind;

        [StringValidation(IsNulOrEmptyAllowed = false, MaxLength = 63)]
        public string Kind
        {
            get { return _kind; }
            set
            {                
                SetProperty(ref _kind, value);
            }
        }

        private double _price;

        [DoublePositiveValidation(ErrorMessage = "Price must be positive.")]
        public double Price
        {
            get { return _price;}
            set
            {
                SetProperty(ref _price, value);
                NotifyOfPropertyChange(() => TotalCost);
            }
        }

        private int _quantity;

        [NumberValidation(Minimum = 0, ErrorMessage = "Quantity must be positive.")]
        public int Quantity
        {
            get { return _quantity; }
            set
            {                
                SetProperty(ref _quantity, value);
                NotifyOfPropertyChange(() => TotalCost);
            }
        }        

        public double TotalCost
        {
            get { return _quantity*_price; }
        }
    }
}
