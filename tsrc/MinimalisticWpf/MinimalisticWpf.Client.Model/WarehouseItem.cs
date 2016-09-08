using MinimalisticWpf.Client.Model.Contracts;

namespace MinimalisticWpf.Client.Model
{
    internal sealed class WarehouseItem : AppModel, IWarehouseItem
    {
        public WarehouseItem(
            string kind,
            double price,
            int quantity)
        {
            Kind = kind;
            Price = price;
            Quantity = quantity;
        }

        public string Kind { get; }

        private double _price;
        public double Price
        {
            get { return _price; }
            set
            {
                if (value == _price)
                {
                    return;
                }
                _price = value;
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
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => TotalCost);
            }
        }

        public double TotalCost
        {
            get { return _quantity * _price; }
        }
    }
}