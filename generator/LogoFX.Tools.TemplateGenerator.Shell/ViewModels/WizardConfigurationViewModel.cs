using System.Windows;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Win32;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public sealed class WizardConfigurationViewModel : CanGenerateViewModel<WizardConfiguration>
    {
        public WizardConfigurationViewModel(WizardConfiguration model) 
            : base(model)
        {
        }

        private ICommand _browseCodeFileCommand;

        public ICommand BrowseCodeFileCommand => _browseCodeFileCommand ??
                                                 (_browseCodeFileCommand = new ActionCommand(BrowseCodeFile));

        public bool IsMultisolution => Model.Solutions.Count > 0;

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