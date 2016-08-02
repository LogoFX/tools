namespace LogoFX.Tools.Templates.Wizard
{
    public sealed class WizardData
    {
        public WizardData()
        {
            FakeData = true;
            Tests = true;
        }

        public bool FakeData { get; set; }

        public bool Tests { get; set; }
    }
}