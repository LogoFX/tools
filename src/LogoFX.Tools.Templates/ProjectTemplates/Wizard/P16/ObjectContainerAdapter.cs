using System;
using System.Collections.Generic;
using Solid.Practices.IoC;

// ReSharper disable once CheckNamespace - copied from 
// https://github.com/godrose/Solid/tree/master/Solid.IoC.Adapters.ObjectContainer
namespace Solid.IoC.Adapters.ObjectContainer
{
    /// <summary>
    /// Represents a container adapter for <see cref="BoDi.ObjectContainer"/>
    /// </summary>
    /// <seealso cref="IIocContainer" />    
    /// <seealso cref="IIocContainerAdapter" />    
    public class ObjectContainerAdapter : IIocContainer, IIocContainerAdapter<BoDi.ObjectContainer>
    {
        private readonly BoDi.ObjectContainer _objectContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectContainerAdapter"/> class.
        /// </summary>
        /// <param name="objectContainer">The object container.</param>
        public ObjectContainerAdapter(BoDi.ObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        /// <summary>
        /// Registers dependency in a transient lifetime style.
        /// </summary>
        /// <typeparam name="TService">Type of dependency declaration.</typeparam>
        /// <typeparam name="TImplementation">Type of dependency implementation.</typeparam>
        public void RegisterTransient<TService, TImplementation>() where TImplementation : class, TService
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers the transient.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RegisterTransient<TService>() where TService : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers the singleton.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        public void RegisterSingleton<TService, TImplementation>() where TImplementation : class, TService
        {
            _objectContainer.RegisterTypeAs<TImplementation, TService>();
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="instance">The instance.</param>
        public void RegisterInstance<TService>(TService instance) where TService : class
        {
            _objectContainer.RegisterInstanceAs(instance);
        }

        /// <summary>
        /// Registers the collection.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="dependencyTypes">The dependency types.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RegisterCollection<TService>(IEnumerable<Type> dependencyTypes) where TService : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers the collection.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="dependencies">The dependencies.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RegisterCollection<TService>(IEnumerable<TService> dependencies) where TService : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers the collection.
        /// </summary>
        /// <param name="dependencyType">Type of the dependency.</param>
        /// <param name="dependencyTypes">The dependency types.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RegisterCollection(Type dependencyType, IEnumerable<Type> dependencyTypes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers the collection.
        /// </summary>
        /// <param name="dependencyType">Type of the dependency.</param>
        /// <param name="dependencies">The dependencies.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RegisterCollection(Type dependencyType, IEnumerable<object> dependencies)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers the handler.
        /// </summary>
        /// <param name="dependencyType">Type of the dependency.</param>
        /// <param name="handler">The handler.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RegisterHandler(Type dependencyType, Func<object> handler)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers the handler.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RegisterHandler<TService>(Func<TService> handler) where TService : class
        {
            _objectContainer.RegisterFactoryAs(handler);
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="dependencyType">Type of the dependency.</param>
        /// <param name="instance">The instance.</param>
        public void RegisterInstance(Type dependencyType, object instance)
        {
            _objectContainer.RegisterInstanceAs(instance, dependencyType);
        }

        /// <summary>
        /// Registers the singleton.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RegisterSingleton(Type serviceType, Type implementationType)
        {
            _objectContainer.RegisterTypeAs(implementationType, serviceType);
        }

        /// <summary>
        /// Registers the transient.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RegisterTransient(Type serviceType, Type implementationType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns></returns>
        public TService Resolve<TService>() where TService : class
        {
            return _objectContainer.Resolve<TService>();
        }

        /// <summary>
        /// Resolves the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        public object Resolve(Type serviceType)
        {
            return _objectContainer.Resolve(serviceType);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            _objectContainer.Dispose();
        }
    }
}
