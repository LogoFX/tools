using $saferootprojectname$.Client.Tests.Contracts.ScreenObjects;
using $safeprojectname$.Infra.Core;

namespace $safeprojectname$.ScreenObjects
{
    public class MainScreenObject : IMainScreenObject
    {
        public StructureHelper StructureHelper { get; set; }

        public MainScreenObject(StructureHelper structureHelper)
        {
            StructureHelper = structureHelper;
        }

        public bool IsActive()
        {
            var main = StructureHelper.GetMain();
            return main.IsActive;
        }
    }
}