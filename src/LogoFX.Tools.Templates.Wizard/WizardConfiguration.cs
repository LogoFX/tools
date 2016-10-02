using System.Collections.Generic;

namespace LogoFX.Tools.Templates.Wizard
{
    public sealed class WizardConfiguration
    {
        public bool TestOption { get; set; }

        public bool FakeOption { get; set; }

        public IEnumerable<SolutionInfo> Solutions { get; set; }
    }
}