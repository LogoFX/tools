using System;
using LogoFX.Client.Mvvm.Model;
using LogoFX.Tools.TemplateGenerator.Model.Contract;

namespace LogoFX.Tools.TemplateGenerator.Model
{
    internal sealed class SolutionConfiguration : Model<Guid>, ISolutionConfiguration
    {
        public string Path { get; }
    }
}