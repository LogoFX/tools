using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto;
using LogoFX.Tools.TemplateGenerator.Engine.Shared;
using Microsoft.Build.Construction;

namespace LogoFX.Tools.TemplateGenerator.Engine.SamplesSpecification
{
    internal sealed partial class TemplateGeneratorEngine
    {
        public string Name => "Samples.Specifications";

        public async Task<SolutionTemplateInfo> CreateSolutionInfoAsync(SolutionConfigurationDto solutionConfiguration)
        {
            SolutionFile solution = SolutionFile.Parse(solutionConfiguration.Path);

            var folders = new Dictionary<Guid, SolutionFolderTemplateInfo>();
            var solutionTemplateInfo = new SolutionTemplateInfo();
            folders.Add(Guid.Empty, solutionTemplateInfo);

            foreach (var proj in solution.ProjectsInOrder)
            {
                await CreateSolutionItemTemplateInfoAsync(solution, solutionTemplateInfo, proj, folders);
            }

            return solutionTemplateInfo;
        }

        public XDocument CreateDefinitionDocument(SolutionConfigurationDto solutionConfiguration)
        {
            return CreateDefinitionInternal(solutionConfiguration);
        }
    }
}