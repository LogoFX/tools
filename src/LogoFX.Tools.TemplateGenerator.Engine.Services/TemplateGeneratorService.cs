using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Tools.TemplateGenerator.Engine.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Engine.Services
{
    [UsedImplicitly]
    internal sealed partial class TemplateGeneratorService : ITemplateGeneratorService
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

        private ITemplateGeneratorEngine GetOrCreateEngine(Guid engineId)
        {
            GetOrCreateEngines();
            return _engines[engineId];
        }
    }
}