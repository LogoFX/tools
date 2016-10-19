using Caliburn.Micro;
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

        //private ICommand _addSolutionCommand;

        //public ICommand AddSolutionCommand
        //{
        //    get
        //    {
        //        return _addSolutionCommand ??
        //               (_addSolutionCommand = ActionCommand
        //                   .When(() => true)
        //                   .Do(() =>
        //                   {
        //                       OpenFileDialog openFileDialog = new OpenFileDialog
        //                       {
        //                           Multiselect = false,
        //                           CheckFileExists = true,
        //                           CheckPathExists = true,
        //                           Filter = "Visual Studio Solution (*.sln)|*sln",
        //                           Title = "Select Solution File"
        //                       };

        //                       var retVal = openFileDialog.ShowDialog() ?? false;

        //                       if (!retVal)
        //                       {
        //                           return;
        //                       }

        //                       var fileName = openFileDialog.FileName;
        //                       var found = Model.Solutions.FirstOrDefault(x => Utils.FileNamesAreEqual(x.FileName, fileName)) != null;

        //                       if (found)
        //                       {
        //                           MessageBox.Show("Solution already added.", "Add Solution", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                       }

        //                       var solution = new SolutionInfo();
        //                       solution.FileName = fileName;
        //                       solution.Name = Utils.SolutionFileNameToName(fileName);
        //                       solution.Caption = solution.Name;
        //                       Model.Solutions.Add(solution);
        //                   }));
        //    }
        //}

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