using System.Windows.Input;
using Caliburn.Micro;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public sealed class WizardConfigurationViewModel : CanGenerateViewModel<WizardConfiguration>
    {
        private readonly IWindowManager _windowManager;
        private WrappingCollection _solutions;

        public WizardConfigurationViewModel(WizardConfiguration model, IWindowManager windowManager) 
            : base(model)
        {
            _windowManager = windowManager;
        }


        private ICommand _addSolutionCommand;

        public ICommand AddSolutionCommand
        {
            get
            {
                return _addSolutionCommand ??
                       (_addSolutionCommand = ActionCommand
                           .When(() => true)
                           .Do(() =>
                           {
                               var createSolutionViewModel = new CreateSolutionViewModel();
                               var retVal = _windowManager.ShowDialog(createSolutionViewModel) ?? false;

                               if (!retVal)
                               {
                                   return;
                               }

                               var solutionInfo = new SolutionInfo
                               {
                                   Name = createSolutionViewModel.Name
                               };
                               Model.Solutions.Add(solutionInfo);
                           }));
            }
        }

        public bool IsMultisolution => Model.Solutions.Count > 1;

        public string Name
        {
            get { return Model.Name; }
            set
            {
                if (value == Model.Name)
                {
                    return;
                }

                Model.Name = value;
                NotifyOfPropertyChange();
                OnCanGenerateUpdated();
            }
        }

        public string Description
        {
            get { return Model.Description; }
            set
            {
                if (value == Model.Description)
                {
                    return;
                }

                Model.Description = value;
                NotifyOfPropertyChange();
            }
        }

        public string DefaultName
        {
            get { return Model.DefaultName; }
            set
            {
                if (value == Model.DefaultName)
                {
                    return;
                }

                Model.DefaultName = value;
                NotifyOfPropertyChange();
                OnCanGenerateUpdated();
            }
        }

        public WrappingCollection Solutions
        {
            get { return _solutions ?? (_solutions = CreateSolutions()); }
        }

        private WrappingCollection CreateSolutions()
        {
            var solutions = new WrappingCollection
            {
                FactoryMethod = o => new SolutionViewModel((SolutionInfo) o, _windowManager)
            };
            solutions.AddSource(Model.Solutions);
            return solutions;
        }

        protected override bool GetCanGenerate()
        {
            return !string.IsNullOrEmpty(Name) &&
                   !string.IsNullOrEmpty(DefaultName) &&
                   Model.Solutions.Count > 0;
        }
    }
}