using JetBrains.Annotations;

namespace LogoFX.Tools.Common.Model
{
    public class SolutionFolderData : SolutionItemData
    {
        public SolutionItemData[] Items { get; [UsedImplicitly] set; }
    }
}