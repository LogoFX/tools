using System;
using EnvDTE;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator.VsExtension.Bridge
{
    internal sealed class DataService : IDataService
    {
        private readonly IServiceProvider _serviceProvider;

        public DataService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public DTE GetDte()
        {
            return (DTE) _serviceProvider.GetService(typeof(DTE));
        }
    }
}