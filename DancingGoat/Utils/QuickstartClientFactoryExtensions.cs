using System;
using System.Linq;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DancingGoat.Utils
{
    internal static class QuickstartClientFactoryExtensions
    {
        /// <summary>
        /// Sets up Delivery <paramref name="options"/> to use either production or preview Delivery API based on
        /// provided <paramref name="projectContext"/>.
        /// </summary>
        internal static IOptionalDeliveryConfiguration SelectApi(this IDeliveryApiConfiguration options, ProjectContext projectContext)
            => projectContext.IsInPreviewMode
                ? options.UsePreviewApi(projectContext.PreviewApiKey)
                : options.UseProductionApi();

        /// <summary>
        /// Registers custom <see cref="DomainBasedDeliveryClientFactory"/> that enables quickstart to use project
        /// context based on domain of the request.
        /// </summary>
        /// <param name="services">Collection of registered service descriptors</param>
        internal static IServiceCollection AddScopedDomainBasedDeliveryClientFactory(this IServiceCollection services)
        {
            var nativeClientFactoryCreator = services.GetCreatorFor<IDeliveryClientFactory>();
            
            return services
                .AddHttpContextAccessor()
                .AddScoped<ProjectContext>()
                .AddScoped<IDeliveryClientFactory>(provider =>
                    new DomainBasedDeliveryClientFactory(
                        nativeClientFactoryCreator(provider),
                        provider.GetService<ProjectContext>(),
                        provider.GetService<ITypeProvider>(),
                        provider.GetService<IContentLinkUrlResolver>()));
        }

        /// <summary>
        /// Returns a function that can instantiate <typeparamref name="TService"/>; provided it was previously
        /// registered in the <paramref name="services"/> collection. If there are multiple registrations (descriptors)
        /// the latest is used. 
        /// </summary>
        /// <param name="services">Collection of registered service descriptors</param>
        /// <typeparam name="TService">Type the creator will instantiate</typeparam>
        /// <returns>
        /// Function that accepts <see cref="IServiceProvider"/> and returns instance of
        /// <typeparamref name="TService"/> or <c>null</c>.
        /// </returns>
        private static Func<IServiceProvider, TService> GetCreatorFor<TService>(this IServiceCollection services)
            where TService : class
            => services
                .LastOrDefault(service => service.ServiceType == typeof(TService))
                .GetCreatorFor<TService>(services);

        /// <summary>
        /// Returns a function that can instantiate <typeparamref name="TService"/>; provided its
        /// <paramref name="descriptor"/> was previously registered in the <paramref name="services"/> collection.
        /// </summary>
        /// <param name="descriptor">
        /// Single descriptor (or <c>null</c>) that represent a <typeparamref name="TService"/> type already registered
        /// in the <paramref name="services"/>.
        /// </param>
        /// <param name="services">Collection of registered service descriptors</param>
        /// <typeparam name="TService">Type the creator will instantiate</typeparam>
        /// <returns>
        /// Function that accepts <see cref="IServiceProvider"/> and returns instance of <typeparamref name="TService"/>
        /// or <c>null</c>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// If the <see cref="ServiceDescriptor"/> contract changed and method is not able to provide valid creator.
        /// </exception>
        /// <remarks>
        /// Only one of service specification implementations (<see cref="ServiceDescriptor.ImplementationInstance"/>,
        /// <see cref="ServiceDescriptor.ImplementationFactory"/>, <see cref="ServiceDescriptor.ImplementationType"/>)
        /// can be specified at a time.
        /// </remarks>
        private static Func<IServiceProvider, TService> GetCreatorFor<TService>(this ServiceDescriptor descriptor, IServiceCollection services)
            where TService : class
            => descriptor switch
            {
                // → no descriptor registered for given service type → empty creator
                null =>
                    _ => null,
                
                // → instance created during start-up → return it directly
                ServiceDescriptor {ImplementationInstance: { }} =>
                    _ => (TService) descriptor.ImplementationInstance,
                
                // → implementation factory specified → use the factory for instantiation
                ServiceDescriptor {ImplementationFactory: { }} => 
                    provider => (TService) descriptor.ImplementationFactory(provider),
                
                // → implementation type specified → instantiate the type
                ServiceDescriptor {ImplementationType: { }} =>
                    descriptor.ImplementationType.GetCreatorFor<TService>(services, descriptor.Lifetime),
                
                // → all implementation instance, factory, and type are not set → descriptor contract have been extended
                _ => throw new NotSupportedException($"Type {typeof(TService).FullName} registered in {nameof(IServiceCollection)} cannot be instantiated by the Quickstart instance creator.")
            };

        /// <summary>
        /// Returns a function that can instantiate <typeparamref name="TService"/> based on provided
        /// <paramref name="implementationType"/>.
        /// </summary>
        /// <param name="implementationType">
        /// Type of instance that will be returned by the creator resulting from this method invocation.
        /// </param>
        /// <param name="services">
        /// Collection of registered service descriptors where the <paramref name="implementationType"/> will be
        /// conditionally registered and readily constructed by invoking the resulting creator afterwards.
        /// </param>
        /// <param name="lifetime">
        /// Lifetime the <paramref name="implementationType"/> should be registered if the type does not have a
        /// descriptor registered in the <paramref name="services"/> yet.
        /// </param>
        /// <typeparam name="TService">Type the creator will instantiate</typeparam>
        /// <returns>
        /// Function that accepts <see cref="IServiceProvider"/> and returns instance of
        /// <typeparamref name="TService"/>. If implementation cannot be instantiated, an
        /// <see cref="InvalidOperationException"/> is thrown. Under an edge-case circumstances, when an
        /// InstantiationFactory is registered and it decides to do so, the <c>null</c> can be returned as well.
        /// </returns>
        private static Func<IServiceProvider, TService> GetCreatorFor<TService>(this Type implementationType, IServiceCollection services, ServiceLifetime lifetime)
            where TService : class
        {
            if (services.All(service => service.ServiceType != implementationType))
            {
                // if type is not already registered, create the reflexive descriptor so the implementation type can
                // later be instantiated by it specifically from service provider. Preserve type of lifetime.
                services.Add(new ServiceDescriptor(
                    implementationType,
                    implementationType,
                    lifetime));
            }

            return provider => (TService) provider.GetRequiredService(implementationType);
        }
    }
}