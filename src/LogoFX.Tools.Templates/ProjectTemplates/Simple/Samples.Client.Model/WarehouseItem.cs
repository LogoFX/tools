using System;
using JetBrains.Annotations;
using $safeprojectname$.Contracts;

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
            Kind = kind;
            Price = price;
            Quantity = quantity;            
        }

        private string _kind;

        public string Kind
        {
            get { return _kind; }
            set
            {
                if (_kind == value)
                {
                    return;
                }

                _kind = value;
                MakeDirty();
                NotifyOfPropertyChange();
            }
        }

        private double _price;
        public double Price
        {
            get { return _price;}
            set
            {
                if (value == _price)
                {
                    return;
                }
                _price = value;
                MakeDirty();
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => TotalCost);
            }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (value == _quantity)
                {
                    return;
                }
                _quantity = value;
                MakeDirty();
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => TotalCost);
            }
        }

        public double TotalCost
        {
            get { return _quantity*_price; }
        }
    }
}
