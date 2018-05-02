using System.Collections.Specialized;
using System.Linq;
using Caliburn.Micro;
using JetBrains.Annotations;
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
        private readonly IViewModelCreatorService _viewModelCreatorService;

        public MainViewModel(
            IConfiguration model,
            IViewModelCreatorService viewModelCreatorService)
        {
            _viewModelCreatorService = viewModelCreatorService;
            Model = model;
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