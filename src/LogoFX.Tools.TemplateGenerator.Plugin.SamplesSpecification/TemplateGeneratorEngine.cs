using System;
using System.Threading.Tasks;
using LogoFX.Tools.TemplateGenerator.Engine.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Plugin.SamplesSpecification
{
    internal sealed class TemplateGeneratorEngine : ITemplateGeneratorEngine
    {
        Task<ISolutionInfo> ITemplateGeneratorEngine.CreateSolutionInfo(string solutionFilename)
        {
            throw new NotImplementedException();
        }
    }
}