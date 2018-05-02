using System.Collections.Generic;

namespace LogoFX.Tools.TemplateGenerator.Model.Contract
{
    public interface IConfiguration
    {
        IEnumerable<ISolutionConfiguration> Solutions { get; }
    }
}