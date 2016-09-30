using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Avalon.Windows.Dialogs;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel;
using LogoFX.Tools.TemplateGenerator.Shell.Properties;
using Microsoft.Win32;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public sealed class WizardConfigurationViewModel : CanGenerateViewModel<WizardConfiguration>
    {
        private WrappingCollection _solutions;

        public WizardConfigurationViewModel(WizardConfiguration model) 
            : base(model)
        {
        }

        private ICommand _browseCodeFileCommand;

        public ICommand BrowseCodeFileCommand
        {
            get
            {
                return _browseCodeFileCommand ??
                       (_browseCodeFileCommand = ActionCommand
                           .When(() => true)
                           .Do(BrowseCodeFile));
            }
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
                               OpenFileDialog openFileDialog = new OpenFileDialog
                               {
                                   Multiselect = false,
                                   CheckFileExists = true,
                                   CheckPathExists = true,
                                   Filter = "Visual Studio Solution (*.sln)|*sln",
                                   Title = "Select Solution File"
                               };

                               var retVal = openFileDialog.ShowDialog() ?? false;

                               if (!retVal)
                               {
                                   return;
                               }

                               var fileName = openFileDialog.FileName;
                               var found = Model.Solutions.FirstOrDefault(x => Utils.FileNamesAreEqual(x.FileName, fileName)) != null;

                               if (found)
                               {
                                   MessageBox.Show("Solution already added.", "Add Solution", MessageBoxButton.OK, MessageBoxImage.Warning);
                               }

                               var solution = new SolutionInfo();
                               solution.FileName = fileName;
                               solution.Name = Utils.SolutionFileNameToName(fileName);
                               solution.Caption = solution.Name;
                               Model.Solutions.Add(solution);
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

        public string CodeFileName
        {
            get { return Model.CodeFileName; }
            private set
            {
                if (Model.CodeFileName == value)
                {
                    return;
                }

                Model.CodeFileName = value;
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
            var solutions = new WrappingCollection();
            solutions.FactoryMethod = o => new SolutionViewModel((SolutionInfo) o);
            solutions.AddSource(Model.Solutions);
            return solutions;
        }

        private void BrowseCodeFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.DefaultExt = ".cs";
            saveFileDialog.FileName = CodeFileName;
            saveFileDialog.Filter = "C# Code File (*.cs)|*.cs";
            saveFileDialog.Title = "Select Solution Wizard File Name";

            var retVal = saveFileDialog.ShowDialog(Application.Current.MainWindow) ?? false;

            if (!retVal)
            {
                return;
            }

            CodeFileName = saveFileDialog.FileName;
        }

        protected override bool GetCanGenerate()
        {
            return !string.IsNullOrEmpty(Name) &&
                   !string.IsNullOrEmpty(DefaultName) &&
                   !string.IsNullOrEmpty(CodeFileName);
        }
    }
}