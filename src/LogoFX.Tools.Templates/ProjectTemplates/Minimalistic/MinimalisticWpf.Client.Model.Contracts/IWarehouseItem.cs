namespace $safeprojectname$
{
    public interface IWarehouseItem
    {
        string Kind { get; }
        double Price { get; set; }
        int Quantity { get; set; }
        double TotalCost { get; }
    }
}