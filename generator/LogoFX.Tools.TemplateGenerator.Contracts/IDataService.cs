using System.Collections;
using EnvDTE;

namespace LogoFX.Tools.TemplateGenerator.Contracts
{
    public interface IDataService
    {
        DTE GetDte();

        IConfiguration LoadConfiguration();

        void SaveConfiguration(IConfiguration configuration);
    }
}