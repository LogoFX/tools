namespace $safeprojectname$
{
    public interface IWarehouseItem : IAppModel
    {
        string Kind { get; }   
        double Price { get; set; }
        int Quantity { get; set; }
        double TotalCost { get; }
    }
}