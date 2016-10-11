using Caliburn.Micro;
using LogoFX.Client.Testing.Contracts;
using LogoFX.Client.Testing.Integration;
using $saferootprojectname$.Client.Data.Fake.ProviderBuilders;
using $saferootprojectname$.Client.Presentation.Shell.ViewModels;
using $saferootprojectname$.Client.Tests.Integration.Infra.Core;

namespace $safeprojectname$
{
    public class StartApplicationService : StartApplicationServiceBase
    {
        public StructureHelper StructureHelper { get; set; }
        private readonly IBuilderRegistrationService _builderRegistrationService;
        private readonly LoginProviderBuilder _loginProviderBuilder;
        private readonly WarehouseProviderBuilder _warehouseProviderBuilder;

        public StartApplicationService(
            IBuilderRegistrationService builderRegistrationService, 
            LoginProviderBuilder loginProviderBuilder,
            WarehouseProviderBuilder warehouseProviderBuilder,
            StructureHelper structureHelper)
        {
            StructureHelper = structureHelper;
            _builderRegistrationService = builderRegistrationService;
            _loginProviderBuilder = loginProviderBuilder;
            _warehouseProviderBuilder = warehouseProviderBuilder;
        }

        // ReSharper disable once RedundantOverridenMember
        protected override void RegisterFakes()
        {
            base.RegisterFakes();
            _builderRegistrationService.RegisterBuilder(_loginProviderBuilder);
            _builderRegistrationService.RegisterBuilder(_warehouseProviderBuilder);            
        }

        protected override void OnStart(object rootObject)
        {
            var shell = (ShellViewModel)rootObject;
            StructureHelper.SetRootObject(shell);
            ScreenExtensions.TryActivate(shell);
            ScreenExtensions.TryActivate(StructureHelper.GetLogin());
        }
    }
}