using System;

namespace LogoFX.Tools.TemplateGenerator.Engine.Shared
{
    public abstract class SolutionItemTemplateInfo
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