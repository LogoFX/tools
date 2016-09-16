namespace LogoFX.Tools.TemplateGenerator
{
    /// <summary>
    /// Information aboud template. Stored in \Definitions\CSharp.vstemplate file.
    /// </summary>
    internal sealed class TemplateDataInfo
    {
        public TemplateDataInfo()
        {
            ProjectType = "CSharp";
            SortOrder = 5000;
            Icon = "icon.png";
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ProjectType { get; set; }
        public string DefaultName { get; set; }
        public int SortOrder { get; set; }
        public string Icon { get; set; }

        public string WizardClassName { get; set; }
    }
}