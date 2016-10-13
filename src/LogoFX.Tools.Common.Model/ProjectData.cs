﻿using System.Collections.Generic;

namespace LogoFX.Tools.Common.Model
{
    public class ProjectData : SolutionItemData
    {
        public string FileName { get; set; }

        public bool IsStartupProject { get; set; }

        public ProjectConfigurationData[] ProjectConfigurations { get; set; }
    }
}