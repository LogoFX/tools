using JetBrains.Annotations;

namespace LogoFX.Tools.Common.Model
{
    [UsedImplicitly]
    public class SolutionData
    {
        public string Name { get; set; }

        public string Caption { get; set; }

        public string IconFileName { get; set; }

        public string PostCreateUrl { get; [UsedImplicitly] set; }

        public SolutionItemData[] Items { get; [UsedImplicitly] set; }
    }
}