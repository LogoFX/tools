using Microsoft.ApplicationInsights;

namespace $safeprojectname$
{    
    sealed partial class App
    {              
        ///<summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
 
        public App()
        {
            WindowsAppInitializer.InitializeAsync();
            InitializeComponent();  
            Initialize();          
        }                 
    }
}