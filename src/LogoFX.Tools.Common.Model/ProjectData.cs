namespace LogoFX.Tools.Common.Model
{
    public class ProjectData : SolutionItemData
    {
        public string FileName { get; set; }

        public bool IsStartupProject { get; set; }

        public string OpenInBrowserItem { get; set; }
    }
}