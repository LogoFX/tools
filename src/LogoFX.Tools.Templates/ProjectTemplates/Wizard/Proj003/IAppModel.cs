using System;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace $safeprojectname$
{    
    public interface IAppModel : IModel<Guid>, IEditableModel
    {
        /// <summary>
        /// Designates whether model should be discarded when cancelling changes
        /// The recommended usage is:
        /// <code>
        /// var model = _dataService.CreateModelAsync();
        /// 
        /// public async Task &lt;Model&gt; CreateModelAsync()
        /// {
        ///     //... wrap into async call
        ///     var dto = _provider.Create();
        ///     var model = Mapper.MapToModel(dto);
        ///     model.IsNew = true;
        ///     return model;
        /// }
        /// 
        /// _dataService.UpdateModelAsync(Model model);
        /// public async Task UpdateModelAsync(Model model)
        /// {
        ///     //... wrap into async call
        ///     var dto = Mapper.MapToDto(model);
        ///     _provider.Update(dto);       
        ///     model.IsNew = false;
        /// }        
        /// 
        /// </code>
        /// </summary>
        bool IsNew { get; set; }
    }
}
