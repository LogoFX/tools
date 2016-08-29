using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Avalon.Windows.Dialogs;
using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Contracts;
using LogoFX.Tools.TemplateGenerator.Shell.Properties;
using Microsoft.Expression.Interactivity.Core;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    [UsedImplicitly]
    public sealed class ShellViewModel : Screen
    {
        #region Fields

        private SolutionTemplateGenerator _solutionTemplateGenerator;
        private readonly IDataService _dataService;

        #endregion

        #region Constructors

        public ShellViewModel()
        {
            _dataService = IoC.Get<IDataService>();
        }

        #endregion

        #region Commands

        private ICommand _browseDestinationFolderCommand;

        public ICommand BrowseDestinationFolderCommand
        {
            get
            {
                return _browseDestinationFolderCommand ??
                       (_browseDestinationFolderCommand = new ActionCommand(() =>
                       {
                           FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
                           {
                               RootSpecialFolder = Environment.SpecialFolder.MyComputer,
                               SelectedPath = DestinationFolder,
                               Title = "Destination Path"
                           };

                           var retVal = folderBrowserDialog.ShowDialog(Application.Current.MainWindow) ?? false;

                           if (!retVal)
                           {
                               return;
                           }

                           DestinationFolder = folderBrowserDialog.SelectedPath;
                       }));
            }
        }

        private ICommand _getInfoCommand;

        public ICommand GetInfoCommand
        {
            get
            {
                return _getInfoCommand ??
                       (_getInfoCommand = new ActionCommand(() =>
                       {
                           _solutionTemplateGenerator = new SolutionTemplateGenerator(
                               SolutionFileName,
                               new TemplateDataInfo
                               {
                                   Name = Name,
                                   Description = Description,
                                   DefaultName = DefaultName,
                               });
                           SolutionTemplateInfo = _solutionTemplateGenerator.GetInfo();
                       }));
            }
        }

        private ICommand _generateTemplateCommand;

        public ICommand GenerateTemplateCommand
        {
            get
            {
                return _generateTemplateCommand ??
                       (_generateTemplateCommand = new ActionCommand(async () =>
                       {
                           if (!Directory.Exists(DestinationFolder) ||
                               SolutionTemplateInfo == null)
                           {
                               return;
                           }

                           IsBusy = true;

                           try
                           {
                               await _solutionTemplateGenerator.GenerateAsync(DestinationFolder, SolutionTemplateInfo);
                               SolutionTemplateInfo = null;
                           }

                           finally
                           {
                               IsBusy = false;
                           }
                       }));
            }
        }

        #endregion

        #region Public Properties

        private string _solutionFileName;

        public string SolutionFileName
        {
            get { return _solutionFileName; }
            private set
            {
                if (value == _solutionFileName)
                {
                    return;
                }

                _solutionFileName = value;
                NotifyOfPropertyChange(() => SolutionFileName);
            }
        }

        private string _destinationFolder;

        public string DestinationFolder
        {
            get { return _destinationFolder; }
            set
            {
                if (value == _destinationFolder)
                {
                    return;
                }

                _destinationFolder = value;
                NotifyOfPropertyChange(() => DestinationFolder);
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                {
                    return;
                }

                _name = value;
                NotifyOfPropertyChange();
            }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value)
                {
                    return;
                }

                _description = value;
                NotifyOfPropertyChange();
            }
        }

        private string _defaultName;

        public string DefaultName
        {
            get { return _defaultName; }
            set
            {
                if (_defaultName == value)
                {
                    return;
                }

                _defaultName = value;
                NotifyOfPropertyChange();
            }
        }

        private ISolutionTemplateInfo _solutionTemplateInfo;

        public ISolutionTemplateInfo SolutionTemplateInfo
        {
            get { return _solutionTemplateInfo; }
            private set
            {
                if (_solutionTemplateInfo == value)
                {
                    return;
                }

                _solutionTemplateInfo = value;
                NotifyOfPropertyChange();
            }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value)
                {
                    return;
                }

                _isBusy = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Overrides

        public override string DisplayName
        {
            get { return "Template Generator"; }
            set { }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            var dte = _dataService.GetDte();
            var solution = dte.Solution;

            if (solution == null || string.IsNullOrEmpty(solution.FullName))
            {
                MessageBox.Show(Application.Current.MainWindow,
                    "Solution not loaded.",
                    DisplayName,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                Task.Run(() =>
                {
                    TryClose();
                });

                return;
            }

            SolutionFileName = solution.FullName;

            var destinationPath = Settings.Default.DestinationPath;
            if (Directory.Exists(destinationPath))
            {
                DestinationFolder = destinationPath;
            }

            Name = Settings.Default.Name;
            Description = Settings.Default.Description;
            DefaultName = Settings.Default.DefaultName;
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            if (close)
            {
                Settings.Default.DestinationPath = DestinationFolder;
                Settings.Default.Name = Name;
                Settings.Default.Description = Description;
                Settings.Default.DefaultName = DefaultName;
                Settings.Default.Save();
            }
        }

        #endregion
    }
}