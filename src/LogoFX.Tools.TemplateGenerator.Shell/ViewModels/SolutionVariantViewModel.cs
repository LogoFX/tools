using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public sealed class SolutionVariantViewModel : ObjectViewModel<SolutionVariant>
    {
        #region Constructors

        public SolutionVariantViewModel(SolutionVariant model)
            : base(model)
        {
            
        }

        #endregion

        #region Commands

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

        public override string DisplayName
        {
            get { return Model.ContainerName; }
            set { }
        }

        private IEnumerable<ProjectViewModel> _projects;

        public IEnumerable<ProjectViewModel> Projects
        {
            get { return _projects; }
            private set
            {
                if (Equals(_projects, value))
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

        #region Public Methods

        public async Task CreateSolutionTemplateInfoAsync()
        {
            var generator = new SolutionTemplateInfoGenerator();
            var solutionTemplateInfo = await generator.GenerateTemplateInfoAsync(Model.SolutionFileName);

            var projects = GetPlainProjects(solutionTemplateInfo.Items)
                .Select(x => new ProjectViewModel(x))
                .ToArray();
            Projects = projects;
        }

        #endregion

        #region Private Members

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