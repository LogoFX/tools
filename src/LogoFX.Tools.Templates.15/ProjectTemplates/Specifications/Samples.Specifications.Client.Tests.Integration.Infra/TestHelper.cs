using Caliburn.Micro;
using $saferootprojectname$.lient.Model.Shared;

namespace $safeprojectname$
{
    public static class TestHelper
    {
        public static void AfterTeardown()
        {
            UserContext.Current = null;                        
            AssemblySource.Instance.Clear();            
        }
    }
}
