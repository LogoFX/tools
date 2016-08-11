namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class TemplateDataInfo
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
    }
}