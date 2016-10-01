using System.Xml.Serialization;
using LogoFX.Tools.Common;
using LogoFX.Tools.TemplateGenerator.Contracts;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class SolutionInfo
    {
        public SolutionInfo()
        {
            
        }

        public SolutionInfo(SolutionInfoDto dto)
        {
            FileName = dto.FileName;
            Name = dto.Name;
            Caption = dto.Caption;
            IconName = dto.IconName;
        }

        public string FileName { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string IconName { get; set; }
        public ISolutionTemplateInfo SolutionTemplateInfo { get; set; }

        public SolutionInfoDto ToDto()
        {
            SolutionInfoDto dto = new SolutionInfoDto
            {
                FileName = FileName,
                Name = Name,
                Caption = Caption,
                IconName = IconName
            };

            return dto;
        }
    }
}