using System.Collections.Generic;
using System.Collections.ObjectModel;
using LogoFX.Tools.Common;

namespace LogoFX.Tools.TemplateGenerator
{
    public sealed class WizardConfiguration
    {
        public WizardConfiguration()
        {
            Solutions = new ObservableCollection<SolutionInfo>();
        }

        public WizardConfiguration(WizardConfigurationDto dto)
            : this()
        {
            TestOption = dto.TestOption;
            FakeOption = dto.FakeOption;
            Name = dto.Name;
            Description = dto.Description;
            DefaultName = dto.DefaultName;
            CodeFileName = dto.CodeFileName;
            IconName = dto.IconName;

            foreach (var solution in dto.Solutions)
            {
                Solutions.Add(new SolutionInfo(solution));
            }
        }

        public bool TestOption { get; set; }

        public bool FakeOption { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string DefaultName { get; set; }

        public string CodeFileName { get; set; }

        public string IconName { get; set; }

        public ObservableCollection<SolutionInfo> Solutions { get; }

        public string ProjectType => "CSharp";

        public int SortOrder => 5000;

        public WizardConfigurationDto ToDto()
        {
            WizardConfigurationDto dto = new WizardConfigurationDto
            {
                TestOption = TestOption,
                FakeOption = FakeOption,
                Name = Name,
                Description = Description,
                DefaultName = DefaultName,
                CodeFileName = CodeFileName,
                IconName = IconName,
                Solutions = new List<SolutionInfoDto>()
            };

            foreach (var solution in Solutions)
            {
                dto.Solutions.Add(solution.ToDto());
            }

            return dto;
        }
    }
}