using Caliburn.Micro;

namespace LogoFX.Tools.Templates.Wizard.ViewModel
{
    public abstract class ObjectViewModel<T> : PropertyChangedBase
    {
        private T _model;

        protected ObjectViewModel(T model)
        {
            Model = model;
        }

        public T Model
        {
            get { return _model; }
            private set
            {
                if (Equals(value, _model))
                {
                    return;
                }

                _model = value;
                NotifyOfPropertyChange();
            }
        }
    }
}