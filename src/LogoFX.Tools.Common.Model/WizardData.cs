using JetBrains.Annotations;

namespace LogoFX.Tools.Common.Model
{
    public class WizardData
    {
        public string Title { get; [UsedImplicitly] set; }

        public SolutionData Solution { get; [UsedImplicitly] set; }
    }
}