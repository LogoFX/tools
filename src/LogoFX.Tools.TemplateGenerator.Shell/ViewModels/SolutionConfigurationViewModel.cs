using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel;
using LogoFX.Client.Mvvm.ViewModel.Contracts;
using LogoFX.Client.Mvvm.ViewModel.Extensions;
using LogoFX.Client.Mvvm.ViewModel.Services;
using LogoFX.Core;
using LogoFX.Tools.TemplateGenerator.Model.Contract;
using LogoFX.Tools.TemplateGenerator.Shared.UIServices;
using SelectionMode = LogoFX.Client.Mvvm.ViewModel.SelectionMode;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    [UsedImplicitly]
    public class SolutionConfigurationViewModel : ScreenObjectViewModel<ISolutionConfiguration>, ICanBeBusy
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

        private ICommand _startGenerationCommand;

        public ICommand StartGenerationCommand
        {
            get
            {
                return _startGenerationCommand ??
                       (_startGenerationCommand = ActionCommand
                           .When(() => CanStartGeneration)
                           .Do(Start)
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
        public WrappingCollection.WithSelection Engines => _engines ?? (_engines = CreateEngines());

        private WrappingCollection.WithSelection CreateEngines()
        {
            var wc = new WrappingCollection.WithSelection(SelectionMode.One)
            {
                FactoryMethod = o => _viewModelCreatorService.CreateViewModel<ITemplateGeneratorEngineInfo, TemplateGeneratorEngineViewModel>((ITemplateGeneratorEngineInfo) o),
                SelectionPredicate = o => string.IsNullOrEmpty(Model.EngineName) || ((TemplateGeneratorEngineViewModel) o).Model.Name == Model.EngineName
            };

            wc.SelectionChanged += WeakDelegate.From((EventHandler) EnginesSelectionChanged);
            wc.AddSource(_dataService.GetAvailableEngines());

            return wc;
        }

        private void EnginesSelectionChanged(object sender, EventArgs e)
        {
            var wc = (WrappingCollection.WithSelection) sender;
            var item = wc.SelectedItem as TemplateGeneratorEngineViewModel;
            Model.EngineName = item?.Model.Name;
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
            string[] propertyArray =
            {
                nameof(ISolutionConfiguration.Path),
                nameof(ISolutionConfiguration.Name),
                nameof(ISolutionConfiguration.EngineName),
                nameof(ISolutionConfiguration.DefaultName),
                nameof(ISolutionConfiguration.TemplateFolder)
            };

            if (e.PropertyName == nameof(ISolutionConfiguration.Name))
            {
                NotifyOfPropertyChange(() => DisplayName);
                NotifyOfPropertyChange(() => TemplatePath);
            }
            else if (e.PropertyName == nameof(ISolutionConfiguration.TemplateFolder))
            {
                NotifyOfPropertyChange(() => TemplatePath);                
            }

            if (propertyArray.Any(x => x == e.PropertyName))
            {
                NotifyOfPropertyChange(() => CanStartGeneration);
            }
        }

        private bool CanStartGeneration => GetCanStartGeneration();

        private bool GetCanStartGeneration()
        {
            return !string.IsNullOrEmpty(Model.Path) &&
                   !string.IsNullOrEmpty(Model.Name) &&
                   !string.IsNullOrEmpty(Model.EngineName) &&
                   !string.IsNullOrEmpty(Model.DefaultName) &&
                   File.Exists(Model.Path);
        }

        private async void Start()
        {
            IsBusy = true;

            try
            {
                await _dataService.GenerateAsync(Model, ((TemplateGeneratorEngineViewModel) Engines.SelectedItem).Model, null);
            }

            finally
            {
                IsBusy = false;
            }
        }

        public override string DisplayName
        {
            get => Model.Name;
            set { }
        }
    }
}