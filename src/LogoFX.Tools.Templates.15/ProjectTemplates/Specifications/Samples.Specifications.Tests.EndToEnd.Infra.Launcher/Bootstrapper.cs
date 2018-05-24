using System;
using System.Collections.Generic;
using System.Linq;
using Solid.Bootstrapping;
using Solid.Extensibility;
using Solid.Practices.Composition;
using Solid.Practices.Composition.Container;
using Solid.Practices.Composition.Contracts;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;
using Solid.Practices.Modularity;

namespace $safeprojectname$
{
    public class Bootstrapper : IInitializable, 
        IExtensible<Bootstrapper>,         
        ICompositionModulesProvider,
        IHaveRegistrator        
    {
        private readonly
            List<IMiddleware<Bootstrapper>>
            _middlewares =
                new List<IMiddleware<Bootstrapper>>();

        public Bootstrapper(IDependencyRegistrator dependencyRegistrator)
        {
            Registrator = dependencyRegistrator;     
        }

        /// <summary>
        /// Gets the list of modules that were discovered during bootstrapper configuration.
        /// </summary>
        /// <value>
        /// The list of modules.
        /// </value>
        public IEnumerable<ICompositionModule> Modules { get; private set; } = new ICompositionModule[] { };

        public IDependencyRegistrator Registrator { get; }

        private void InitializeCompositionModules()
        {
            var compositionManager = new CompositionManager();
            try
            {
                compositionManager.Initialize(".", new string[] { });
            }
            catch (AggregateAssemblyInspectionException)
            {

            }
            finally
            {
                Modules = compositionManager.Modules.ToArray();
            }                        
        }

        /// <summary>
        /// Extends the functionality by using the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public Bootstrapper Use(
            IMiddleware<Bootstrapper> middleware)
        {
            _middlewares.Add(middleware);
            return this;
        }

        public void Initialize()
        {
            InitializeCompositionModules();
            MiddlewareApplier.ApplyMiddlewares(this, _middlewares);
        }        
    }
}