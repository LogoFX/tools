using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public class ConfigurationViewModel
    {
        private readonly IConfiguration _model;

        public ConfigurationViewModel(IConfiguration model)
        {
            _model = model;
        }

        public IConfiguration Model
        {
            get { return _model; }
        }
    }
}