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

        public DateTime Time
        {
            get { return Model.Time; }
        }

        public string Message
        {
            get { return Model.Message; }
        }
    }
}