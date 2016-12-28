using Caliburn.Micro;
using $saferootprojectname$.Client.Model.Shared;

namespace $safeprojectname$
{
    public static class TestHelper
    {
        public static void BeforeTeardown()
        {
            //TODO:
        }

        public static void AfterTeardown()
        {
            UserContext.Current = null;                        
            AssemblySource.Instance.Clear();            
        }
    }
}
