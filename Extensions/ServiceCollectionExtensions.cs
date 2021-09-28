using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ModularWPFTemplate.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Attempts to build the ServiceProvider from a registered IServiceProviderFactory
        /// </summary>
        /// <param name="collection">Service Collection that hosts the IServiceProviderFactory and that will be build</param>
        /// <returns>Built ServiceProvider, Default if no factory found</returns>
        public static IServiceProvider BuildFromFactory(this IServiceCollection collection)
        {
            // Construct the Default Microsost ServiceProvider
            var provider = collection.BuildServiceProvider();
            // Get the service provider factory from the services
            var factory = provider.GetService<IServiceProviderFactory<IServiceCollection>>();

            // If a factory is resolved and it's not the default
            if (factory != null && !(factory is DefaultServiceProviderFactory))
            {
                using (provider)
                {
                    // Build and return the Provider from the factory
                    return factory.CreateServiceProvider(factory.CreateBuilder(collection));
                }
            }

            // Otherwise return vanilla provider
            return provider;
        }

        /// <summary>
        /// Removes a service from the service collection by the specified type
        /// </summary>
        /// <typeparam name="T">Type of the service to remove from the service collection</typeparam>
        /// <param name="services">Service Collection to remove service from</param>
        /// <returns>Passed service collection for method chaining</returns>
        public static IServiceCollection Remove<T>(this IServiceCollection services)
        {
            return services.Remove(typeof(T));
        }

        /// <summary>
        /// Removes a service from the service collection by the specified type
        /// </summary>
        /// <param name="services">Service Collection to remove service from</param>
        /// <param name="serviceType">Type of the service to remove from the service collection</param>
        /// <returns>Passed service collection for method chaining</returns>
        public static IServiceCollection Remove(this IServiceCollection services, Type serviceType)
        {
            // Attempt to find the associated service descriptor for the type
            var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == serviceType);

            // If it does exist
            if(serviceDescriptor != null)
            {
                // Remove service
                services.Remove(serviceDescriptor);
            }

            // Return service for method chaining
            return services;
        }

        /// <summary>
        /// Creates a shallow copy of the provided IServiceCollection
        /// </summary>
        /// <param name="services">The service collection to clone</param>
        /// <returns>The cloned IServiceCollection</returns>
        public static IServiceCollection Clone(this IServiceCollection services)
        {
            // Create a new service collection to hold the copy
            var copy = new ServiceCollection();

            // Iterate over the descriptors contained in the collection
            foreach(var serviceDescriptor in services)
            {
                // Add service descriptor to copy
                copy.Add(serviceDescriptor);
            }

            // Return populated copy
            return copy;
        }

        /// <summary>
        /// Add instantiable types from the provided assembly as Transient
        /// services to the provided service collection.
        /// 
        /// If no assembly is provided then the currently executing assembly is
        /// used.
        /// </summary>
        /// <typeparam name="ServiceType">The parent type used to represent all the registered services</typeparam>
        /// <param name="services">Service Collection to add types to</param>
        /// <param name="assembly">Assembly to resolve the types from</param>
        /// <param name="filter">Callback for modifying the resolved type list</param>
        /// <returns></returns>
        public static IServiceCollection AddTransientsFromAssembly<ServiceType>(this IServiceCollection services,
            Assembly assembly = null,
            Func<IEnumerable<Type>, IEnumerable<Type>> filter = null)
        {
            // Chain call to a more specific overload
            return services.AddAssemblyTypes<ServiceType>(
                assembly,
                ServiceLifetime.Transient,
                filter);
        }

        /// <summary>
        /// Add instantiable types from the provided assembly as Transient
        /// services to the provided service collection.
        /// 
        /// If no assembly is provided then the currently executing assembly is
        /// used.
        /// 
        /// If no service type is provided then all services are registered as
        /// themselves.
        /// </summary>
        /// <param name="services">Service Collection to add types to</param>
        /// <param name="assembly">Assembly to resolve the types from</param>
        /// <param name="serviceType">The parent type used to represent all the registered services</param>
        /// <param name="filter">Callback for modifying the resolved type list</param>
        /// <returns>The original IServiceCollection for method chaining</returns>
        public static IServiceCollection AddTransientsFromAssembly(this IServiceCollection services,
            Assembly assembly = null,
            Type serviceType = null,
            Func<IEnumerable<Type>, IEnumerable<Type>> filter = null)
        {
            // Chain call to a more specific overload
            return services.AddAssemblyTypes(
                assembly,
                serviceType,
                ServiceLifetime.Transient,
                filter);
        }

        /// <summary>
        /// Add instantiable types from the provided assembly as singleton
        /// services to the provided service collection.
        /// 
        /// If no assembly is provided then the currently executing assembly is
        /// used.
        /// </summary>
        /// <typeparam name="ServiceType">The parent type used to represent all the registered services</typeparam>
        /// <param name="services">Service Collection to add types to</param>
        /// <param name="assembly">Assembly to resolve the types from</param>
        /// <param name="filter">Callback for modifying the resolved type list</param>
        /// <returns></returns>
        public static IServiceCollection AddSingletonsFromAssembly<ServiceType>(this IServiceCollection services,
            Assembly assembly = null,
            Func<IEnumerable<Type>, IEnumerable<Type>> filter = null)
        {
            // Chain call to a more specific overload
            return services.AddAssemblyTypes<ServiceType>(
                assembly,
                ServiceLifetime.Singleton,
                filter);
        }

        /// <summary>
        /// Add instantiable types from the provided assembly as singleton
        /// services to the provided service collection.
        /// 
        /// If no assembly is provided then the currently executing assembly is
        /// used.
        /// 
        /// If no service type is provided then all services are registered as
        /// themselves.
        /// </summary>
        /// <param name="services">Service Collection to add types to</param>
        /// <param name="assembly">Assembly to resolve the types from</param>
        /// <param name="serviceType">The parent type used to represent all the registered services</param>
        /// <param name="filter">Callback for modifying the resolved type list</param>
        /// <returns>The original IServiceCollection for method chaining</returns>
        public static IServiceCollection AddSingletonsFromAssembly(this IServiceCollection services,
            Assembly assembly = null,
            Type serviceType = null,
            Func<IEnumerable<Type>, IEnumerable<Type>> filter = null)
        {
            // Chain call to a more specific overload
            return services.AddAssemblyTypes(
                assembly,
                serviceType,
                ServiceLifetime.Singleton,
                filter);
        }

        /// <summary>
        /// Add instantiable types from the provided assembly as scoped
        /// services to the provided service collection.
        /// 
        /// If no assembly is provided then the currently executing assembly is
        /// used.
        /// </summary>
        /// <typeparam name="ServiceType">The parent type used to represent all the registered services</typeparam>
        /// <param name="services">Service Collection to add types to</param>
        /// <param name="assembly">Assembly to resolve the types from</param>
        /// <param name="filter">Callback for modifying the resolved type list</param>
        /// <returns></returns>
        public static IServiceCollection AddScopedsFromAssembly<ServiceType>(this IServiceCollection services,
            Assembly assembly = null,
            Func<IEnumerable<Type>, IEnumerable<Type>> filter = null)
        {
            // Chain call to a more specific overload
            return services.AddAssemblyTypes<ServiceType>(
                assembly,
                ServiceLifetime.Scoped,
                filter);
        }

        /// <summary>
        /// Add instantiable types from the provided assembly as scoped
        /// services to the provided service collection.
        /// 
        /// If no assembly is provided then the currently executing assembly is
        /// used.
        /// 
        /// If no service type is provided then all services are registered as
        /// themselves.
        /// </summary>
        /// <param name="services">Service Collection to add types to</param>
        /// <param name="assembly">Assembly to resolve the types from</param>
        /// <param name="serviceType">The parent type used to represent all the registered services</param>
        /// <param name="filter">Callback for modifying the resolved type list</param>
        /// <returns>The original IServiceCollection for method chaining</returns>
        public static IServiceCollection AddScopedsFromAssembly(this IServiceCollection services,
            Assembly assembly = null,
            Type serviceType = null,
            Func<IEnumerable<Type>, IEnumerable<Type>> filter = null)
        {
            // Chain call to a more specific overload
            return services.AddAssemblyTypes(
                assembly,
                serviceType,
                ServiceLifetime.Scoped,
                filter);
        }

        /// <summary>
        /// Add instantiable types from the provided assembly as services to the
        /// provided service collection.
        /// 
        /// If no assembly is provided then the currently executing assembly is
        /// used.
        /// </summary>
        /// <typeparam name="ServiceType">The parent type used to represent all the registered services</typeparam>
        /// <param name="services">Service Collection to add types to</param>
        /// <param name="assembly">Assembly to resolve the types from</param>
        /// <param name="lifetime">Lifetime scope used to register the service types</param>
        /// <param name="filter">Callback for modifying the resolved type list</param>
        /// <returns>The original IServiceCollection for method chaining</returns>
        public static IServiceCollection AddAssemblyTypes<ServiceType>(this IServiceCollection services,
            Assembly assembly = null,
            ServiceLifetime lifetime = ServiceLifetime.Transient,
            Func<IEnumerable<Type>, IEnumerable<Type>> filter = null)
        {
            // Chain call to a more specific overload
            return services.AddAssemblyTypes(
                assembly,
                typeof(ServiceType),
                lifetime,
                filter);
        }

        /// <summary>
        /// Add instantiable types from the provided assembly as services to the
        /// provided service collection.
        /// 
        /// If no assembly is provided then the currently executing assembly is
        /// used.
        /// 
        /// If no service type is provided then all services are registered as
        /// themselves.
        /// </summary>
        /// <param name="services">Service Collection to add types to</param>
        /// <param name="assembly">Assembly to resolve the types from</param>
        /// <param name="serviceType">The parent type used to represent all the registered services</param>
        /// <param name="lifetime">Lifetime scope used to register the service types</param>
        /// <param name="filter">Callback for modifying the resolved type list</param>
        /// <returns>The original IServiceCollection for method chaining</returns>
        public static IServiceCollection AddAssemblyTypes(this IServiceCollection services,
            Assembly assembly = null,
            Type serviceType = null,
            ServiceLifetime lifetime = ServiceLifetime.Transient,
            Func<IEnumerable<Type>, IEnumerable<Type>> filter = null)
        {
            // If no assembly provided
            if (assembly == null)
            {
                // Use the currently executing assembly
                assembly = Assembly.GetExecutingAssembly();
            }

            // Chain call to a more specific overload
            return services.AddAssemblyTypes(
                new Assembly[] { assembly },
                serviceType,
                lifetime,
                filter);
        }

        /// <summary>
        /// Add instantiable types from provided assemblies as services to the
        /// provided service collection.
        /// </summary>
        /// <typeparam name="ServiceType">The parent type used to represent all the registered services</typeparam>
        /// <param name="services">Service Collection to add types to</param>
        /// <param name="assemblies">Assemblies to resolve the types from</param>
        /// <param name="lifetime">Lifetime scope used to register the service types</param>
        /// <param name="filter">Callback for modifying the resolved type list</param>
        /// <returns>The original IServiceCollection for method chaining</returns>
        public static IServiceCollection AddAssemblyTypes<ServiceType>(this IServiceCollection services,
            Assembly[] assemblies,
            ServiceLifetime lifetime = ServiceLifetime.Transient,
            Func<IEnumerable<Type>, IEnumerable<Type>> filter = null)
        {
            // Chain call to a more specific overload
            return services.AddAssemblyTypes(
                assemblies,
                typeof(ServiceType),
                lifetime,
                filter);
        }

        /// <summary>
        /// Add instantiable types from provided assemblies as services to the
        /// provided service collection.
        /// 
        /// If no service type is provided then all services are registered as
        /// themselves.
        /// </summary>
        /// <param name="services">Service Collection to add types to</param>
        /// <param name="assemblies">Assemblies to resolve the types from</param>
        /// <param name="serviceType">The parent type used to represent all the registered services</param>
        /// <param name="lifetime">Lifetime scope used to register the service types</param>
        /// <param name="filter">Callback for modifying the resolved type list</param>
        /// <returns>The original IServiceCollection for method chaining</returns>
        public static IServiceCollection AddAssemblyTypes(this IServiceCollection services,
            Assembly[] assemblies,
            Type serviceType = null,
            ServiceLifetime lifetime = ServiceLifetime.Transient,
            Func<IEnumerable<Type>, IEnumerable<Type>> filter = null)
        {
            Func<Type, ServiceDescriptor> descriptorFactory;

            // If a service type was provided
            if (serviceType != null)
            {
                // Ensure the found types are assignable to the provided type
                Func<IEnumerable<Type>, IEnumerable<Type>> typeFilter = typeList =>
                    typeList.Where(t => serviceType.IsAssignableFrom(t));

                // If a filter is already present
                if (filter != null)
                {
                    // Merge the type filter and the provided filter
                    filter = typeList => filter.Invoke(typeFilter.Invoke(typeList));
                }
                // Otherwise
                else
                {
                    // Just assign the type filter
                    filter = typeFilter;
                }

                // Use a factory that creates a service descriptor for the
                // provided service type and resolved type
                descriptorFactory = t => new ServiceDescriptor(serviceType, t, lifetime);
            }
            // Otherwise not
            else
            {
                // Use a factory that creates a service descriptor for the
                // implementation as itself
                descriptorFactory = t => new ServiceDescriptor(t, t, lifetime);
            }

            // Chain call to a more specific overload
            return services.AddAssemblyTypes(
                assemblies,
                descriptorFactory,
                filter);
        }

        /// <summary>
        /// Add instantiable types from provided assemblies as services to the
        /// provided service collection.
        /// </summary>
        /// <param name="services">Service Collection to add types to</param>
        /// <param name="assemblies">Assemblies to resolve the types from</param>
        /// <param name="descriptorFactory">Callback for creating service descriptors for each type</param>
        /// <param name="filter">Callback for modifying the resolved type list</param>
        /// <returns>The original IServiceCollection for method chaining</returns>
        public static IServiceCollection AddAssemblyTypes(this IServiceCollection services,
            Assembly[] assemblies,
            Func<Type, ServiceDescriptor> descriptorFactory,
            Func<IEnumerable<Type>, IEnumerable<Type>> filter = null)
        {
            // Contains the list of available assembly services
            var typeList = new List<Type>();

            // Iterate over the provided assemblies
            foreach (var assembly in assemblies)
            {
                // Get the instantiable types from the assembly
                var assemblyTypes = assembly.GetTypes().Where(t => !t.IsAbstract);

                // Add the types to the general list
                typeList.AddRange(assemblyTypes);
            }

            // If a filter is present
            if (filter != null)
            {
                // Filter the service list
                typeList = filter.Invoke(typeList).ToList();
            }

            // Iterate over the resolved type list
            foreach (var type in typeList)
            {
                // Add each type's created service descriptor to the service
                // collection
                services.Add(descriptorFactory.Invoke(type));
            }

            // Return the original service collection for method chaining
            return services;
        }
    }
}
