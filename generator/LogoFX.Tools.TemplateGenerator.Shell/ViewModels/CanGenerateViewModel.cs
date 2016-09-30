using System;
using LogoFX.Client.Mvvm.ViewModel;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public abstract class CanGenerateViewModel<T> : ObjectViewModel<T>
    {
        protected CanGenerateViewModel(T model)
            : base(model)
        {
        }

        public event EventHandler CanGenerateUpdated = delegate { };

        public bool CanGenerate => GetCanGenerate();

        protected void OnCanGenerateUpdated()
        {
            CanGenerateUpdated(this, EventArgs.Empty);
        }

        protected abstract bool GetCanGenerate();
    }
}