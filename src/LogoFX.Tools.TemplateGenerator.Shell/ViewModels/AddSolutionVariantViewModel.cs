using System.IO;
using System.Windows.Input;
using Caliburn.Micro;
using LogoFX.Client.Mvvm.Commanding;
using Microsoft.Win32;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public class AddSolutionVariantViewModel : Screen
    {
        public override string DisplayName
        {
            get { return "Add new Solution Variant"; }
            set { }
        }

        private ICommand _browseSolutionFileCommand;

        public ICommand BrowseSolutionFileCommand
        {
            get
            {
                return _browseSolutionFileCommand ??
                       (_browseSolutionFileCommand = ActionCommand
                           .When(() => true)
                           .Do(() =>
                           {
                               var openFileDialog = new OpenFileDialog
                               {
                                   Multiselect = false,
                                   CheckFileExists = true,
                                   CheckPathExists = true,
                                   DefaultExt = ".sln",
                                   Filter = "Visual Studio Solution (*.sln)|*.sln",
                                   Title = "Browse Solution File",
                                   ValidateNames = true
                               };

                               if (!string.IsNullOrEmpty(SolutionFileName))
                               {
                                   var folder = Path.GetDirectoryName(SolutionFileName);

                                   if (!string.IsNullOrEmpty(folder))
                                   {
                                       openFileDialog.InitialDirectory = folder;
                                   }
                               }

                               var retVal = openFileDialog.ShowDialog() ?? false;

                               if (!retVal)
                               {
                                   return;
                               }

                               SolutionFileName = openFileDialog.FileName;
                           }));
            }
        }

        private ICommand _okCommand;

        public ICommand OkCommand
        {
            get
            {
                return _okCommand ??
                       (_okCommand = ActionCommand
                           .When(() => !string.IsNullOrEmpty(SolutionFileName))
                           .Do(() =>
                           {
                               TryClose(true);
                           })
                           .RequeryOnPropertyChanged(this, () => SolutionFileName));
            }
        }

        private ICommand _cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ??
                       (_cancelCommand = ActionCommand
                           .When(() => true)
                           .Do(() =>
                           {
                               TryClose(false);
                           }));
            }
        }

        private string _containerName;

        public string ContainerName
        {
            get { return _containerName; }
            set
            {
                if (_containerName == value)
                {
                    return;
                }

                _containerName = value;
                NotifyOfPropertyChange();
            }
        }

        private string _solutionFileName;

        public string SolutionFileName
        {
            get { return _solutionFileName; }
            private set
            {
                if (_solutionFileName == value)
                {
                    return;
                }

                _solutionFileName = value;
                NotifyOfPropertyChange();
            }
        }
    }
}