using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel.Contracts;
using LogoFX.Client.Mvvm.ViewModel.Services;
using LogoFX.Core;
using LogoFX.Tools.TemplateGenerator.Model.Contract;
using LogoFX.Tools.TemplateGenerator.Shell.Properties;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    [UsedImplicitly]
    public class MainViewModel : Conductor<SolutionConfigurationViewModel>.Collection.OneActive, IModelWrapper<IConfiguration>
    {
        private readonly IDataService _dataService;
        private readonly IViewModelCreatorService _viewModelCreatorService;

        public MainViewModel(
            IConfiguration model,
            IDataService dataService,
            IViewModelCreatorService viewModelCreatorService)
        {
            _dataService = dataService;
            _viewModelCreatorService = viewModelCreatorService;
            Model = model;
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
                               var newName = "New Solution ";
                               int index = 1;
                               // ReSharper disable once AccessToModifiedClosure
                               while (Items.Any(x => x.Model.Name == newName + index))
                               {
                                   ++index;
                               }

                               var solution = _dataService.AddSolution(newName + index);
                               var solutionVm = Items.Single(x => x.Model == solution);
                               ActivateItem(solutionVm);
                           }));
            }
        }

        private ICommand _removeSolutionCommand;

        public ICommand RemoveSolutionCommand
        {
            get
            {
                return _removeSolutionCommand ??
                       (_removeSolutionCommand = ActionCommand
                           .When(() => ActiveItem != null)
                           .Do(() =>
                           {
                               _dataService.RemoveSolution(ActiveItem.Model);
                           })
                           .RequeryOnPropertyChanged(this, () => ActiveItem));
            }
        }

        private void CreateSolutions()
        {
            Items.AddRange(Model.Solutions.Select(CreateSolutionConfiguration));
            var selectedSolutionName = Settings.Default.SelectedSolutionName;

            if (string.IsNullOrEmpty(selectedSolutionName))
            {
                return;
            }

            var selectedSolution = Items.FirstOrDefault(x => x.Model.Name == selectedSolutionName);
            if (selectedSolution == null && Items.Count > 0)
            {
                selectedSolution = Items[0];
            }

            ActivateItem(selectedSolution);
        }

        private SolutionConfigurationViewModel CreateSolutionConfiguration(ISolutionConfiguration solutionConfiguration)
        {
            return _viewModelCreatorService.CreateViewModel<ISolutionConfiguration, SolutionConfigurationViewModel>(solutionConfiguration);
        }

        private void OnSolutionsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                Items.RemoveRange(Items.Where(x => e.OldItems.OfType<ISolutionConfiguration>().Contains(x.Model)).ToList());
            }

            if (e.NewItems != null)
            {
                Items.AddRange(e.NewItems.OfType<ISolutionConfiguration>().Select(CreateSolutionConfiguration));
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            CreateSolutions();

            if (Model.Solutions is INotifyCollectionChanged collection)
            {
                collection.CollectionChanged += WeakDelegate.From(OnSolutionsCollectionChanged);
            }
        }

        protected override void ChangeActiveItem(SolutionConfigurationViewModel newItem, bool closePrevious)
        {
            base.ChangeActiveItem(newItem, closePrevious);
            Settings.Default.SelectedSolutionName = newItem?.Model.Name;
        }

        object IModelWrapper.Model => Model;

        public IConfiguration Model { get; private set; }
    }
}