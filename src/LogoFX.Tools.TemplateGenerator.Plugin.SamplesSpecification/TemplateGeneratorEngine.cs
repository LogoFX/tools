using System.Threading.Tasks;
using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Engine.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Plugin.SamplesSpecification
{
    [UsedImplicitly]
    internal sealed class TemplateGeneratorEngine : ITemplateGeneratorEngine
    {
        public string Name => "Samples.Specification";
        
        public Task<ISolutionInfo> CreateSolutionInfo(string solutionFilename)
        {
            throw new System.NotImplementedException();
        }
    }
}