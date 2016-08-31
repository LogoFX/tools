using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Avalon.Windows.Dialogs;
using Caliburn.Micro;
using EnvDTE;
using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Contracts;
using LogoFX.Tools.TemplateGenerator.Shell.Properties;
using Microsoft.Expression.Interactivity.Core;
using Window = System.Windows.Window;

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
                           FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                           folderBrowserDialog.RootSpecialFolder = Environment.SpecialFolder.MyComputer;
                           //folderBrowserDialog.SelectedPath = DestinationFolder;
                           folderBrowserDialog.Title = "Destination Path";

                           var retVal = folderBrowserDialog.ShowDialog() ?? false;

                           if (!retVal)
                           {
                               return;
                           }

                           //DestinationFolder = folderBrowserDialog.SelectedPath;
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
                           //if (!Directory.Exists(DestinationFolder) ||
                           //    SolutionTemplateInfo == null)
                           //{
                           //    return;
                           //}

                           //IsBusy = true;

                           //try
                           //{
                           //    await _solutionTemplateGenerator.GenerateAsync(DestinationFolder, SolutionTemplateInfo);
                           //    SolutionTemplateInfo = null;
                           //}

                           //finally
                           //{
                           //    IsBusy = false;
                           //}
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

        private ConfigurationViewModel _activeConfiguration;

        public ConfigurationViewModel ActiveConfiguration
        {
            get { return _activeConfiguration; }
            private set
            {
                if (_activeConfiguration == value)
                {
                    return;
                }

                _activeConfiguration = value;
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

        #region Private Members

        private void GetInfo(Solution solution)
        {
            var configuration = _dataService.LoadConfiguration();
            ActiveConfiguration = new ConfigurationViewModel(configuration);
            _dataService.SaveConfiguration(configuration);

            //_solutionTemplateGenerator = new SolutionTemplateGenerator(
            //    solution,
            //    new TemplateDataInfo
            //    {
            //        Name = Name,
            //        Description = Description,
            //        DefaultName = DefaultName,
            //    });
            //SolutionTemplateInfo = _solutionTemplateGenerator.GetInfo();
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
            //if (Directory.Exists(destinationPath))
            //{
            //    DestinationFolder = destinationPath;
            //}

            //Name = Settings.Default.Name;
            //Description = Settings.Default.Description;
            //DefaultName = Settings.Default.DefaultName;

            GetInfo(solution);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            //if (close)
            //{
            //    Settings.Default.DestinationPath = DestinationFolder;
            //    Settings.Default.Name = Name;
            //    Settings.Default.Description = Description;
            //    Settings.Default.DefaultName = DefaultName;
            //    Settings.Default.Save();
            //}
        }

        #endregion
    }
}