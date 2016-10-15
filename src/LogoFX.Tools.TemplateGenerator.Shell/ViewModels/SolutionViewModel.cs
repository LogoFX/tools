using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel;
using LogoFX.Client.Mvvm.ViewModel.Contracts;
using LogoFX.Core;
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

        //private ICommand _startupProjectChangedCommand;

        //public ICommand StartupProjectChangedCommand
        //{
        //    get
        //    {
        //        return _startupProjectChangedCommand ??
        //               (_startupProjectChangedCommand = ActionCommand<SelectionChangedEventArgs>
        //                   .When(e => true)
        //                   .Do(e =>
        //                   {
        //                       var startupPorject = (ProjectViewModel) e.AddedItems.FirstOrDefault();
        //                       if (startupPorject == null)
        //                       {
        //                           return;
        //                       }
        //                       Projects.Select(startupPorject);
        //                   }));
        //    }
        //}

        private ICommand _projectsComboBoxLoaded;

        public ICommand ProjectsComboBoxLoaded
        {
            get
            {
                return _projectsComboBoxLoaded ??
                       (_projectsComboBoxLoaded = ActionCommand
                           .When(() => true)
                           .Do(() =>
                           {
                               var startupProject = Projects.SingleOrDefault(x => x.Name == Model.StartupProjectName) ??
                                                    Projects.First();
                               startupProject.IsStartup = true;
                               StartupProject = startupProject;
                           }));
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

        //private WrappingCollection.WithSelection _projects;
        //public WrappingCollection.WithSelection Projects
        //{
        //    get { return _projects ?? (_projects = CreateProjects()); }
        //}

        private IEnumerable<ProjectViewModel> _projects;

        public IEnumerable<ProjectViewModel> Projects
        {
            get { return _projects; }
            private set
            {
                if (_projects == value)
                {
                    return;
                }

                _projects = value;
                NotifyOfPropertyChange();
            }
        }

        private ProjectViewModel _startupProject;

        public ProjectViewModel StartupProject
        {
            get { return _startupProject; }
            set
            {
                if (_startupProject == value)
                {
                    return;
                }

                _startupProject = value;
                NotifyOfPropertyChange();

                Model.StartupProjectName = _startupProject.Name;

                if (_startupProject.IsStartup)
                {
                    return;
                }

                foreach (ProjectViewModel project in Projects)
                {
                    project.IsStartup = false;
                }
                _startupProject.IsStartup = true;
            }
        }

        #endregion

        #region Private Members

        //private WrappingCollection.WithSelection CreateProjects()
        //{
        //    var result = new WrappingCollection.WithSelection
        //    {
        //        FactoryMethod = o => new ProjectViewModel((IProjectTemplateInfo) o)
        //    };

        //    var projects = GetPlainProjects(SolutionTemplateInfo.Items).ToArray();
        //    result.AddSource(projects);
        //    result.SelectionHandler = (o, args) =>
        //    {
        //        var startupProject = (ProjectViewModel)args.Item;

        //        Model.StartupProjectName = startupProject.Name;

        //        if (startupProject.IsStartup)
        //        {
        //            return;
        //        }

        //        foreach (ProjectViewModel project in Projects)
        //        {
        //            project.IsStartup = false;
        //        }
        //        startupProject.IsStartup = true;
        //    };

        //    Task.Run(() =>
        //    {
        //        Thread.Sleep(100);
        //        Execute.BeginOnUIThread(() =>
        //        {
        //            var startupProject = result.OfType<ProjectViewModel>().SingleOrDefault(x => x.Name == Model.StartupProjectName) ??
        //                                 result.OfType<ProjectViewModel>().First();
        //            startupProject.IsStartup = true;
        //            result.Select(startupProject);
        //        });
        //    });

        //    return result;
        //}

        private async void CreateSolutionTemplateInfo()
        {
            IsBusy = true;

            try
            {
                SolutionTemplateInfoGenerator generator = new SolutionTemplateInfoGenerator();
                SolutionTemplateInfo = await generator.GenerateTemplateInfoAsync(Model.FileName);

                var projects = GetPlainProjects(SolutionTemplateInfo.Items)
                    .Select(x => new ProjectViewModel(x))
                    .ToArray();
                Projects = projects;
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