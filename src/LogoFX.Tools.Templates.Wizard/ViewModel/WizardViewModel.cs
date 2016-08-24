using System.Windows.Input;
using Caliburn.Micro;
using LogoFX.Client.Mvvm.Commanding;

namespace LogoFX.Tools.Templates.Wizard.ViewModel
{
    public sealed class WizardViewModel : PropertyChangedBase
    {
        private bool _fakeData = true;
        private bool _tests = true;
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title)
                {
                    return;
                }

                _title = value;
                NotifyOfPropertyChange();
            }
        }

        public bool FakeData
        {
            get { return _fakeData; }
            set
            {
                if (value == _fakeData)
                {
                    return;
                }

                _fakeData = value;
                NotifyOfPropertyChange();
            }
        }

        public bool Tests
        {
            get { return _tests; }
            set
            {
                if (value == _tests)
                {
                    return;
                }

                _tests = value;
                NotifyOfPropertyChange();
            }
        }
    }
}