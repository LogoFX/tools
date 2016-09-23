using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using $saferootprojectname$.Client.Presentation.Shell.ViewModels;

namespace $safeprojectname$
{
    public class AppBootstrapper : BootstrapperContainerBase<ExtendedSimpleContainerAdapter>.WithRootObject<ShellViewModel>
    {
        private static readonly ExtendedSimpleContainerAdapter _iocContainer = new ExtendedSimpleContainerAdapter();

        public AppBootstrapper() : base(_iocContainer)
        {
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {            
            RegisterNavigationService(rootFrame);
        }

        private void RegisterNavigationService(Frame rootFrame, bool treatViewAsLoaded = false, bool cacheViewModels = false)
        {
            INavigationService navigationService = cacheViewModels ? new CachingFrameAdapter(rootFrame, treatViewAsLoaded) : new FrameAdapter(rootFrame, treatViewAsLoaded);
            _iocContainer.RegisterInstance(navigationService);
        }
    }    
}