using System;
using LogoFX.Client.Mvvm.ViewModel;
using $saferootprojectname$.Client.Model.Contracts;

namespace $safeprojectname$.ViewModels
{
    public sealed class EventViewModel : ObjectViewModel<IEvent>
    {
        public EventViewModel(IEvent model)
            : base(model)
        {

        }

        public DateTime Time => Model.Time;

        public string Message => Model.Message;
    }
}