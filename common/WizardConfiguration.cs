using System.Collections.Generic;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class WizardConfiguration
    {
        public bool TestOption { get; set; }

        public bool FakeOption { get; set; }

        public Dictionary<string, string> Solutions { get; set; }
    }
}