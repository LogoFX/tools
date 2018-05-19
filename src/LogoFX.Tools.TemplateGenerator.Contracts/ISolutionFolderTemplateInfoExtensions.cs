using System;
using System.Collections.Generic;
using System.Linq;

namespace LogoFX.Tools.TemplateGenerator.Contracts
{
    // ReSharper disable once InconsistentNaming
    public static class ISolutionFolderTemplateInfoExtensions
    {
        public static IEnumerable<IProjectTemplateInfo> GetProjectsPlain(this IEnumerable<ISolutionTemplateInfo> solutionTemplates)
        {
            return solutionTemplates.SelectMany(x => x.GetProjectsPlain()).Distinct();
        }

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