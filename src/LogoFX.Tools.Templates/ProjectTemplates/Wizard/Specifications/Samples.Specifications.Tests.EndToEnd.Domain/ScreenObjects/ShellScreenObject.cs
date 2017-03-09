using $saferootprojectname$.Tests.Domain.ScreenObjects;

namespace $safeprojectname$.ScreenObjects
{
    class ShellScreenObject : IShellScreenObject
    {
        public StructureHelper StructureHelper { get; set; }

        public ShellScreenObject(StructureHelper structureHelper)
        {
            StructureHelper = structureHelper;
        }

        public void Close()
        {            
            var shell = StructureHelper.GetShell();
            shell.Close();
        }
    }
}
