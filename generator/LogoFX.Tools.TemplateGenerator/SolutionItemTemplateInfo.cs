using System;

namespace LogoFX.Tools.TemplateGenerator
{
    internal abstract class SolutionItemTemplateInfo : ISolutionItemTemplateInfo
    {
        protected SolutionItemTemplateInfo(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
    }
}