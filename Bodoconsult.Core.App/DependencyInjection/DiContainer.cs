// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Microsoft.Extensions.DependencyInjection;

namespace Bodoconsult.Core.App.DependencyInjection
{
    /// <summary>
    /// Central class for managing DI container
    /// </summary>
    public class DiContainer
    {

        /// <summary>
        /// Default ctor
        /// </summary>
        public DiContainer()
        {
            ServiceCollection = new ServiceCollection();
        }


        public ServiceCollection ServiceCollection { get; }


        public IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Create the service provider. Must be called after <see cref="ServiceCollection"/> is fully configured.
        /// </summary>
        public void BuildServiceProvider()
        {
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }


        /// <summary>
        /// Get an instance of a certain type
        /// </summary>
        /// <typeparam name="T">Requested type to resolve</typeparam>
        /// <returns>Object instance of the requested type</returns>
        public T Get<T>()
        {
            try
            {
                return (T)ServiceProvider.GetService(typeof(T));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(typeof(T).Name, e);
            }
        }


        /// <summary>
        /// Add a service scoped. Scoped lifetime services (AddScoped) are created once per client request (connection).
        ///           When using a scoped service in a middleware, inject the service into the Invoke or InvokeAsync method.
        /// Don't inject via constructor injection because it forces the service to behave like a singleton. 
        /// </summary>
        /// <typeparam name="TInterface">Service interface</typeparam>
        /// <typeparam name="TInstanceType">Instance type implementing service interface</typeparam>
        public void AddScoped<TInterface, TInstanceType>() where TInterface : class where TInstanceType : class, TInterface
        {
            ServiceCollection.AddScoped<TInterface, TInstanceType>();
        }

        /// <summary>
        /// Add a service transient. Transient lifetime services are created each time they're requested from the service container. This lifetime works best for lightweight, stateless services.
        /// </summary>
        /// <typeparam name="TInterface">Service interface</typeparam>
        /// <typeparam name="TInstanceType">Instance type implementing service interface</typeparam>
        public void AddTransient<TInterface, TInstanceType>() where TInterface : class where TInstanceType : class, TInterface
        {
            ServiceCollection.AddTransient<TInterface, TInstanceType>();
        }


        /// <summary>
        /// Add aservice as a singleton
        /// </summary>
        /// <typeparam name="TInterface">Service interface</typeparam>
        /// <typeparam name="TInstanceType">Instance type implementing service interface</typeparam>
        public void AddSingleton<TInterface, TInstanceType>() where TInterface : class where TInstanceType : class, TInterface
        {
            ServiceCollection.AddSingleton<TInterface, TInstanceType>();
        }



        /// <summary>
        /// Add a instance as singleton
        /// </summary>
        /// <typeparam name="TInterface">Service interface</typeparam>
        /// <param name="instance">Concrete object implementing service interface</param>
        public void AddSingletonInstance<TInterface>(TInterface instance) where TInterface : class
        {
            ServiceCollection.AddSingleton(instance);
        }


        /// <summary>
        /// Clear all (intended for testing purposes)
        /// </summary>
        public void ClearAll()
        {
            ServiceProvider = null;
            ServiceCollection.Clear();
        }


        /// <summary>
        /// Adds a singleton service of the type specified with an
        /// instance specified in <paramref name="instance"/> to the
        /// specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="instance">Instance to use as singleton</param>
        public void AddSingleton<TInterface>(TInterface instance) where TInterface : class
        {
            ServiceCollection.AddSingleton(instance);
        }


        /// <summary>
        /// Adds a singleton service of the type specified with an
        /// instance specified in <paramref name="instance"/> to the
        /// specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="instance">Instance to use as singleton</param>
        public void AddSingleton(Type instance)
        {
            ServiceCollection.AddSingleton(instance);
        }


    }
}
