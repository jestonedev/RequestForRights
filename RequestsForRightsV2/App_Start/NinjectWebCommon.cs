using RequestsForRights.Database;
using RequestsForRights.Database.Repositories;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain;
using RequestsForRightsV2.Infrastructure.Services;
using RequestsForRightsV2.Infrastructure.Services.Interfaces;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(RequestsForRightsV2.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(RequestsForRightsV2.App_Start.NinjectWebCommon), "Stop")]

namespace RequestsForRightsV2.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IDatabaseContext>().To<DatabaseContext>();
            kernel.Bind<IRequestRepository>().To<RequestRepository>();
            kernel.Bind<IResourceGroupRepository>().To<ResourceGroupRepository>();
            kernel.Bind<ISecurityRepository>().To<SecurityRepository>();
            kernel.Bind<IRequestService>().To<RequestService>();
            kernel.Bind<IResourceGroupService>().To<ResourceGroupService>();
            kernel.Bind<ISecurityService>().To<SecurityService>();
        }        
    }
}
