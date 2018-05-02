using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel.Extensions;
using LogoFX.Core;
using LogoFX.Tools.TemplateGenerator.Model.Contract;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    [UsedImplicitly]
    public class SolutionConfigurationViewModel : ScreenObjectViewModel<ISolutionConfiguration>
    {
        private readonly IDataService _dataService;

        public SolutionConfigurationViewModel(ISolutionConfiguration model, IDataService dataService)
            : base(model)
        {
            _dataService = dataService;
            model.PropertyChanged += WeakDelegate.From(OnModelPropertyChanged);
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

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ISolutionConfiguration.Name))
            {
                NotifyOfPropertyChange(() => DisplayName);
            }
        }

        public override string DisplayName
        {
            get { return Model.Name;}
            set { }
        }
    }
}