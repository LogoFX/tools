namespace MinimalisticWpf.Client.Model.Contracts
{
    public interface IWarehouseItem
    {
        string Kind { get; }
        double Price { get; set; }
        int Quantity { get; set; }
        double TotalCost { get; }
    }
}