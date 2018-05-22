using System;
using JetBrains.Annotations;

namespace LogoFX.Tools.TemplateGenerator.Data.Contracts.Dto
{
    public abstract class BaseAppDto
    {
        [UsedImplicitly] 
        private Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}