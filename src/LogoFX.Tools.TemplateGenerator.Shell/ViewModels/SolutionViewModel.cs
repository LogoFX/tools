using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel;
using LogoFX.Client.Mvvm.ViewModel.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public sealed class SolutionViewModel : ObjectViewModel<SolutionInfo>, ICanBeBusy
    {
        #region Fields

        private readonly IWindowManager _windowManager;

        #endregion

        #region Constructors

        public SolutionViewModel(SolutionInfo model, IWindowManager windowManager)
            : base(model)
        {
            _windowManager = windowManager;
            CreateSolutionVariants();
        }

        #endregion

        #region Commands

        private ICommand _browseIconCommand;

        public ICommand BrowseIconCommand
        {
            get
            {
                return _browseIconCommand ??
                       (_browseIconCommand = ActionCommand
                           .When(() => false)
                           .Do(() => { }));
            }
        }

        private ICommand _addSolutionVariantCommand;

        public ICommand AddSolutionVariantCommand
        {
            get
            {
                return _addSolutionVariantCommand ??
                       (_addSolutionVariantCommand = ActionCommand
                           .When(() => true)
                           .Do(() =>
                           {
                               var addSolutionVariantVm = new AddSolutionVariantViewModel();
                               var retVal = _windowManager.ShowDialog(addSolutionVariantVm) ?? false;
                               if (!retVal)
                               {
                                   return;
                               }

                               var solutionVariant = new SolutionVariant
                               {
                                   ContainerName = addSolutionVariantVm.ContainerName,
                                   SolutionFileName = addSolutionVariantVm.SolutionFileName
                               };
                               var solutionVariantVm = new SolutionVariantViewModel(solutionVariant);
                               _solutionVariants.Add(solutionVariantVm);
                               SelectedSolutionVariant = solutionVariantVm;
                           }));
            }
        }

        private ICommand _removeSolutionVariantCommand;

        public ICommand RemoveSolutionVariantCommand
        {
            get
            {
                return _removeSolutionVariantCommand ??
                       (_removeSolutionVariantCommand = ActionCommand
                           .When(() => SelectedSolutionVariant != null)
                           .Do(() =>
                           {
                               _solutionVariants.Remove(SelectedSolutionVariant);
                               SelectedSolutionVariant = SolutionVariants.FirstOrDefault();
                           })
                           .RequeryOnPropertyChanged(this, () => SelectedSolutionVariant));
            }
        }

        #endregion

        #region Public Properties

        private readonly ObservableCollection<SolutionVariantViewModel> _solutionVariants =
            new ObservableCollection<SolutionVariantViewModel>();

        public IEnumerable<SolutionVariantViewModel> SolutionVariants
        {
            get { return _solutionVariants; }
        }

        private SolutionVariantViewModel _selectedSolutionVariant;

        public SolutionVariantViewModel SelectedSolutionVariant
        {
            get { return _selectedSolutionVariant; }
            set
            {
                if (_selectedSolutionVariant == value)
                {
                    return;
                }

                _selectedSolutionVariant = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Private Members

        private async void CreateSolutionVariants()
        {
            IsBusy = true;

            try
            {
                _solutionVariants.Clear();
                foreach (var solutionVariant in Model.SolutionVariants)
                {
                    var solutionVariantVm = new SolutionVariantViewModel(solutionVariant);
                    await solutionVariantVm.CreateSolutionTemplateInfoAsync();
                    _solutionVariants.Add(solutionVariantVm);
                }

                SelectedSolutionVariant = SolutionVariants.FirstOrDefault();
            }

            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}