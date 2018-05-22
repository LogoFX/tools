using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;
using LogoFX.Tools.TemplateGenerator.Engine.Contracts;
using LogoFX.Tools.TemplateGenerator.Engine.Shared;
using Microsoft.Build.Evaluation;

namespace LogoFX.Tools.TemplateGenerator.Engine.Services
{
    [UsedImplicitly]
    internal sealed class TemplateGeneratorService : ITemplateGeneratorService
    {
        private IDictionary<Guid, ITemplateGeneratorEngine> _engines;

        private void CreateEngines()
        {
            _engines = IoC.GetAll<ITemplateGeneratorEngine>().ToDictionary(x => Guid.NewGuid(), y => y);
        }

        private void GetOrCreateEngines()
        {
            if (_engines == null)
            {
                CreateEngines();
            }
        }

        public TemplateGeneratorEngineInfoDto[] GetAvailableEngines()
        {
            GetOrCreateEngines();

            return _engines
                .Select(x => new TemplateGeneratorEngineInfoDto
                {
                    Id = x.Key,
                    Name = x.Value.Name
                })
                .ToArray();
        }

        public async Task<ProjectInfoDto[]> GetProjects(SolutionConfigurationDto solutionConfiguration, Guid engineId)
        {
            var result = await CreateProjectsAsync(solutionConfiguration.Path, _engines[engineId]);
            ProjectCollection.GlobalProjectCollection.UnloadAllProjects();
            return result;
        }

        private async Task<ProjectInfoDto[]> CreateProjectsAsync(string solutionFileName, ITemplateGeneratorEngine engine)
        {
            var solutionTemplateInfo = await engine.CreateSolutionInfoAsync(solutionFileName);
            return GetPlainProjects(solutionTemplateInfo.Items)
                .Select(x => new ProjectInfoDto {Name = x.Name})
                .ToArray();
        }

        private IEnumerable<ProjectTemplateInfo> GetPlainProjects(IEnumerable<SolutionItemTemplateInfo> items)
        {
            var result = new List<ProjectTemplateInfo>();

            foreach (var item in items)
            {
                if (item is SolutionFolderTemplateInfo folder)
                {
                    result.AddRange(GetPlainProjects(folder.Items));
                    continue;
                }

                if (item is ProjectTemplateInfo project)
                {
                    result.Add(project);
                    continue;
                }

                throw new ArgumentException($"Unknown Solution Item type '{item.GetType().Name}'.");
            }

            return result;
        }
    }
}