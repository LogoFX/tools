using System.Collections.Generic;

namespace LogoFX.Tools.Common.Model
{
    public sealed class ProjectConfigurationsData
    {
        public string Name { get; set; }

        public List<ProjectConfigurationData> Configurations { get; set; }
    }
}