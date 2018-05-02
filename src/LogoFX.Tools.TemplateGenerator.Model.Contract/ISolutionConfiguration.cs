    using System;
    using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Tools.TemplateGenerator.Model.Contract
{
    public interface ISolutionConfiguration : IModel<Guid>
    {
        string Path { get; }
    }
}