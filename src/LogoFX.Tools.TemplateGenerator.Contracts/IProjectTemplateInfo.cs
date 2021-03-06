﻿using System.Collections.Generic;

namespace LogoFX.Tools.TemplateGenerator.Contracts
{
    public interface IProjectTemplateInfo : ISolutionItemTemplateInfo
    {
        string NameWithoutRoot { get; }

        string FileName { get; }

        string DestinationFileName { get; }

        void SetDestinationFileName(string destinationFileName);

        IEnumerable<IProjectConfiguration> ProjectConfigurations { get; }

        bool IsStartup { get; set; }
    }
}