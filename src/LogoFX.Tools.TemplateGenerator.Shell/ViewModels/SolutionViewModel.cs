using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
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
            CreateSolutionTemplateInfo();
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

        public ISolutionTemplateInfo SolutionTemplateInfo
        {
            get { return Model.SolutionTemplateInfo; }
            private set
            {
                if (SolutionTemplateInfo == value)
                {
                    return;
                }

                Model.SolutionTemplateInfo = value;
                NotifyOfPropertyChange();
            }
        }

        private WrappingCollection.WithSelection _projects;
        public WrappingCollection.WithSelection Projects
        {
            get { return _projects ?? (_projects = CreateProjects()); }
        }

        #endregion

        #region Private Members

        private WrappingCollection.WithSelection CreateProjects()
        {
            var result = new WrappingCollection.WithSelection
            {
                FactoryMethod = o => new ProjectViewModel((IProjectTemplateInfo) o)
            };

            var projects = GetPlainProjects(SolutionTemplateInfo.Items);
            result.AddSource(projects);

            Task.Run(() =>
            {
                Thread.Sleep(100);
                Execute.BeginOnUIThread(() =>
                {
                    var startupProject = result.OfType<ProjectViewModel>().SingleOrDefault(x => x.IsStartup);
                    if (startupProject == null)
                    {
                        startupProject = result.OfType<ProjectViewModel>().First();
                        startupProject.IsStartup = true;
                    }
                    result.Select(startupProject);
                });
            });

            return result;
        }

        private async void CreateSolutionTemplateInfo()
        {
            IsBusy = true;

            try
            {
                SolutionTemplateInfoGenerator generator = new SolutionTemplateInfoGenerator();
                SolutionTemplateInfo = await generator.GenerateTemplateInfoAsync(Model.FileName);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private IEnumerable<IProjectTemplateInfo> GetPlainProjects(IEnumerable<ISolutionItemTemplateInfo> items)
        {
            var result = new List<IProjectTemplateInfo>();

            foreach (var item in items)
            {
                var folder = item as ISolutionFolderTemplateInfo;
                if (folder != null)
                {
                    result.AddRange(GetPlainProjects(folder.Items));
                    continue;
                }

                var project = item as IProjectTemplateInfo;
                if (project != null)
                {
                    result.Add(project);
                    continue;
                }

                throw new ArgumentException($"Unknown Solution Item type '{item.GetType().Name}'.");
            }

            return result;
        }

        #endregion
    }
}