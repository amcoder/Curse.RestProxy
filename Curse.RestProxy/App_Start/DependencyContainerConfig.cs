using System;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.WebApi;

namespace Curse.RestProxy
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class DependencyContainerConfig
    {
        /// <summary>Configure the dependency container</summary>
        public static void Configure(HttpConfiguration config)
        {
            var container = new UnityContainer();
            RegisterTypes(container);

            config.DependencyResolver = new UnityHierarchicalDependencyResolver(container);
        }

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<LoginService.IClientLoginService, LoginService.ClientLoginServiceClient>(
                new HierarchicalLifetimeManager(),
                new InjectionConstructor("BinaryHttpsClientLoginServiceEndpoint"));

            container.RegisterType<AddOnService.IAddOnService, AddOnService.AddOnServiceClient>(
                new HierarchicalLifetimeManager(),
                new InjectionConstructor("BinaryHttpsAddOnServiceEndpoint"));
        }
    }
}
