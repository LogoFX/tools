using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        private ICommand _editSolutionCommand;

        public ICommand EditSolutionCommand
        {
            get
            {
                return _editSolutionCommand ??
                       (_editSolutionCommand = ActionCommand
                           .When(() => true)
                           .Do(() =>
                           {
                               var createSolutionViewModel = new CreateSolutionViewModel(Model);
                               var retVal = _windowManager.ShowDialog(createSolutionViewModel) ?? false;

                               if (!retVal)
                               {
                                   return;
                               }

                               Model.Name = createSolutionViewModel.Name;
                               Model.Caption = createSolutionViewModel.Caption;
                           }));
            }
        }

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
                           .Do(async () =>
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

                               IsBusy = true;

                               try
                               {
                                   await solutionVariantVm.CreateSolutionTemplateInfoAsync();
                               }
                               finally
                               {
                                   IsBusy = false;
                               }

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

        private ObservableCollection<SolutionVariantViewModel> _solutionVariants;

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
                _solutionVariants = new ObservableCollection<SolutionVariantViewModel>();
                _solutionVariants.CollectionChanged += (sender, args) =>
                {
                    Model.SolutionVariants = _solutionVariants
                        .Select(x => x.Model)
                        .ToArray();
                };
                foreach (var solutionVariant in Model.SolutionVariants)
                {
                    if (!File.Exists(solutionVariant.SolutionFileName))
                    {
                        continue;
                    }

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