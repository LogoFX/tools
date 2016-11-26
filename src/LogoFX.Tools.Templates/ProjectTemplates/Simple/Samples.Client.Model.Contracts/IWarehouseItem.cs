using System;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace $safeprojectname$
{
    public interface IWarehouseItem : IAppModel, IEditableModel
    {
        string Kind { get; }   
        double Price { get; set; }
        int Quantity { get; set; }
        double TotalCost { get; }
    }
}