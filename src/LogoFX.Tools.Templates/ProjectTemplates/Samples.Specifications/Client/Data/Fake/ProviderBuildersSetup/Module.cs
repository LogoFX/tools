using Attest.Fake.Core;
using Attest.Fake.Moq;
using JetBrains.Annotations;
using Solid.Practices.Modularity;

namespace $safeprojectname$
{    
    [UsedImplicitly]    
    class Module : IPlainCompositionModule
    {
        public void RegisterModule()
        {
            FakeFactoryContext.Current = new FakeFactory();
            ConstraintFactoryContext.Current = new ConstraintFactory();
        }
    }
}
