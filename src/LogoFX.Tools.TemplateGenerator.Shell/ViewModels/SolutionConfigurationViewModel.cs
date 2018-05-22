using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel;
using LogoFX.Client.Mvvm.ViewModel.Extensions;
using LogoFX.Client.Mvvm.ViewModel.Services;
using LogoFX.Core;
using LogoFX.Tools.TemplateGenerator.Model.Contract;
using LogoFX.Tools.TemplateGenerator.Shared.UIServices;
using SelectionMode = LogoFX.Client.Mvvm.ViewModel.SelectionMode;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    [UsedImplicitly]
    public class SolutionConfigurationViewModel : ScreenObjectViewModel<ISolutionConfiguration>
    {
        private readonly IDataService _dataService;
        private readonly IViewModelCreatorService _viewModelCreatorService;

        public SolutionConfigurationViewModel(
            ISolutionConfiguration model, 
            IDataService dataService,
            IViewModelCreatorService viewModelCreatorService)
            : base(model)
        {
            _dataService = dataService;
            _viewModelCreatorService = viewModelCreatorService;
            model.PropertyChanged += WeakDelegate.From(OnModelPropertyChanged);
        }

        private async void Start()
        {
            var projects = await _dataService.GetProjectsAsync(Model, ((TemplateGeneratorEngineViewModel) Engines.SelectedItem).Model);
        }

        private ICommand _startGenerationCommand;

        public ICommand StartGenerationCommand
        {
            get
            {
                return _startGenerationCommand ??
                       (_startGenerationCommand = ActionCommand
                           .When(() => true)
                           .Do(() =>
                           {
                               Start();
                           })
                           .RequeryOnPropertyChanged(this, () => CanStartGeneration));
            }
        }

        private ICommand _browseSolutionPathCommand;

        public ICommand BrowseSolutionPathCommand
        {
            get
            {
                return _browseSolutionPathCommand ??
                       (_browseSolutionPathCommand = ActionCommand
                           .When(() => true)
                           .Do(() =>
                           {
                               var openFileDialog = new OpenFileDialog
                               {
                                   Multiselect = false,
                                   CheckFileExists = true,
                                   CheckPathExists = true,
                                   DefaultExt = ".sln",
                                   Filter = @"Visual Studio Solution (*.sln)|*.sln",
                                   Title = @"Browse Solution File",
                                   ValidateNames = true
                               };

                               if (!string.IsNullOrEmpty(Model.Path))
                               {
                                   var folder = Path.GetDirectoryName(Model.Path);
                                   openFileDialog.InitialDirectory = folder;
                               }

                               var retVal = openFileDialog.ShowDialog() == DialogResult.OK;

                               if (!retVal)
                               {
                                   return;
                               }

                               _dataService.SetSolutionPath(Model, openFileDialog.FileName);
                           }));
            }
        }

        private ICommand _browseTemplateFolderCommand;

        public ICommand BrowseTemplateFolderCommand
        {
            get
            {
                return _browseTemplateFolderCommand ??
                       (_browseTemplateFolderCommand = ActionCommand
                           .When(() => true)
                           .Do(() =>
                           {
                               FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
                               {
                                   SelectedPath = Model.TemplateFolder,
                                   Description = @"Destination Path"
                               };

                               var retVal = FolderBrowserLauncher.ShowFolderBrowser(folderBrowserDialog) == DialogResult.OK;

                               if (!retVal)
                               {
                                   return;
                               }

                               Model.TemplateFolder = folderBrowserDialog.SelectedPath;
                           }));
            }
        }

        private WrappingCollection.WithSelection _engines;

        public WrappingCollection.WithSelection Engines
        {
            get { return _engines ?? (_engines = CreateEngines()); }
        }

        private WrappingCollection.WithSelection CreateEngines()
        {
            var wc = new WrappingCollection.WithSelection(SelectionMode.One)
            {
                FactoryMethod = o => _viewModelCreatorService.CreateViewModel<ITemplateGeneratorEngineInfo, TemplateGeneratorEngineViewModel>((ITemplateGeneratorEngineInfo) o),
                SelectionPredicate = o => string.IsNullOrEmpty(Model.PluginName) || ((TemplateGeneratorEngineViewModel) o).Model.Name == Model.PluginName
            };

            wc.AddSource(_dataService.GetAvailableEngines());

            return wc;
        }

        public string TemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(Model.TemplateFolder) || string.IsNullOrEmpty(Model.Name))
                {
                    return string.Empty;
                }

                return Path.Combine(Model.TemplateFolder, Model.Name);
            }
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ISolutionConfiguration.Name))
            {
                NotifyOfPropertyChange(() => DisplayName);
                NotifyOfPropertyChange(() => TemplatePath);
            }
            else if (e.PropertyName == nameof(ISolutionConfiguration.TemplateFolder))
            {
                NotifyOfPropertyChange(() => TemplatePath);
            }
        }

        private bool CanStartGeneration
        {
            get { return GetCanStartGeneration(); }
        }

        private bool GetCanStartGeneration()
        {
            if (string.IsNullOrEmpty(Model.Path) || !File.Exists(Model.Path))
            {
                return false;
            }

            if (string.IsNullOrEmpty(Model.Name))
            {
                return false;
            }

            return true;
        }

        public override string DisplayName
        {
            get => Model.Name;
            set { }
        }
    }
}