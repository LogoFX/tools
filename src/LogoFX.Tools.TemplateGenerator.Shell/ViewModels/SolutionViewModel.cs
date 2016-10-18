using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel;
using LogoFX.Client.Mvvm.ViewModel.Contracts;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public sealed class SolutionViewModel : ObjectViewModel<SolutionInfo>, ICanBeBusy
    {
        #region Constructors

        public SolutionViewModel(SolutionInfo model)
            : base(model)
        {
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

        #endregion

        #region Public Properties

        private IEnumerable<SolutionVariantViewModel> _containers;

        public IEnumerable<SolutionVariantViewModel> Containers
        {
            get { return _containers; }
            private set
            {
                if (Equals(_containers, value))
                {
                    return;
                }

                _containers = value;
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
                var solutionVariantList = new List<SolutionVariantViewModel>();
                foreach (var solutionVariant in Model.SolutionVariants)
                {
                    var solutionVariantVM = new SolutionVariantViewModel(solutionVariant);
                    await solutionVariantVM.CreateSolutionTemplateInfoAsync();
                    solutionVariantList.Add(solutionVariantVM);
                }

                Containers = solutionVariantList;
            }

            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}