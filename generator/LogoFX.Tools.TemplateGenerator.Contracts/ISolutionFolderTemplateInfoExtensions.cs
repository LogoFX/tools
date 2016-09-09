using System;
using System.Collections.Generic;

namespace LogoFX.Tools.TemplateGenerator.Contracts
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
                    var projectInfo = (IProjectTemplateInfo) item;
                    if (projectInfo.Id != Guid.Empty)
                    {
                        result.Add(projectInfo);
                    }
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