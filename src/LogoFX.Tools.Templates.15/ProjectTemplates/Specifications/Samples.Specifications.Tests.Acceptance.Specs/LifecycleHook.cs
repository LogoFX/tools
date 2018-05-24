using BoDi;
using $saferootprojectname$.pecifications.Tests.Domain;
using $saferootprojectname$.pecifications.Tests.EndToEnd.Infra.Launcher;
using Solid.IoC.Adapters.ObjectContainer;
using Solid.Practices.Composition;
using Solid.Practices.IoC;
using TechTalk.SpecFlow;

namespace $safeprojectname$
{
    [Binding]
    sealed class LifecycleHook
    {
        private readonly IIocContainer _iocContainer;

        public LifecycleHook(ObjectContainer objectContainer)
        {
            _iocContainer = new ObjectContainerAdapter(objectContainer);
        }

        [BeforeTestRun]
        public static void BeforeAllScenarios()
        {
            PlatformProvider.Current = new NetStandardPlatformProvider();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _iocContainer.Initialize();
            var setupService = _iocContainer.Resolve<ISetupService>();
            setupService.Setup();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _iocContainer.Teardown();
        }
    }
}
