using System.Collections.Generic;

namespace LogoFX.Tools.TemplateGenerator
{
    public static class ISolutionFolderTemplateInfoExtensions
    {
        public static IEnumerable<IProjectTemplateInfo> GetProjectsPlain(this ISolutionFolderTemplateInfo solutionFolder)
        {
            List<IProjectTemplateInfo> result = new List<IProjectTemplateInfo>();
            foreach (var item in solutionFolder.Items)
            {
                ISolutionFolderTemplateInfo folder = item as ISolutionFolderTemplateInfo;
                if (folder == null)
                {
                    result.Add((IProjectTemplateInfo)item);
                }
                else
                {
                    result.AddRange(GetProjectsPlain(folder));
                }
            }

            return result;
        }

    }
}