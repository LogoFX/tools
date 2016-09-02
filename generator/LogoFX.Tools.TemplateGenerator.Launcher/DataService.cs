using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Launcher
{
    internal sealed class DataService : IDataService
    {
        public string GetSolutionFileName()
        {
            return @"c:\Projects\LogoUI\LogoFX\Samples.Specifications\Specifications.sln";
        }

        public IConfiguration LoadConfiguration()
        {
            throw new System.NotImplementedException();
        }

        public void SaveConfiguration(IConfiguration configuration)
        {
            throw new System.NotImplementedException();
        }
    }
}