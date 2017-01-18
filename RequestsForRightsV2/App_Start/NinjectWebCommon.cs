using System;
using System.Configuration;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using RequestsForRights;
using RequestsForRights.Database;
using RequestsForRights.Database.Repositories;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Infrastructure.Security;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Ldap;
using RequestsForRights.Models.Models;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace RequestsForRights
{
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
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
            kernel.Bind<IDatabaseContext>().To<DatabaseContext>().InRequestScope();
            // Repositories
            kernel.Bind<IRequestRepository>().To<RequestRepository>();
            kernel.Bind<IResourceGroupRepository>().To<ResourceGroupRepository>();
            kernel.Bind<IResourceRepository>().To<ResourceRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<ISecurityRepository>().To<SecurityRepository>().InRequestScope();
            kernel.Bind<ILdapRepository>().ToConstant(
                new LdapRepository(
                    ConfigurationManager.AppSettings["ldap_username"],
                    ConfigurationManager.AppSettings["ldap_password"]));
            // Data services
            kernel.Bind<IRequestService<RequestUserModel>>().To<RequestService<RequestUserModel>>();
            kernel.Bind<IRequestService<RequestDelegatePermissionsUserModel>>().
                To<RequestDelegatePermissionsService>();
            kernel.Bind<IResourceGroupService>().To<ResourceGroupService>();
            kernel.Bind<IResourceService>().To<ResourceService>();
            kernel.Bind<IUserService>().To<UserService>();
            // Security services
            kernel.Bind<IResourceGroupSecurityService>().To<ResourceGroupSecurityService>();
            kernel.Bind<IResourceSecurityService>().To<ResourceSecurityService>();
            kernel.Bind<IRequestSecurityService<RequestUserModel>>().To<RequestSecurityService<RequestUserModel>>();
            kernel.Bind<IRequestSecurityService<RequestDelegatePermissionsUserModel>>().
                To<RequestSecurityService<RequestDelegatePermissionsUserModel>>();
            kernel.Bind<IUserSecurityService>().To<UserSecurityService>();
        }        
    }
}
