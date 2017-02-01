using System;
using System.Configuration;
using System.Net.Mail;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using RequestsForRights;
using RequestsForRights.CachePool;
using RequestsForRights.Database;
using RequestsForRights.Database.Repositories;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Infrastructure.Security;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Infrastructure.Utilities.EmailNotify;
using RequestsForRights.Ldap;
using RequestsForRights.Models.Models;
using RequestsForRights.Models.ViewModels.Request;

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
            kernel.Bind<ICachePool>().To<CachePool.CachePool>();
            // Repositories
            kernel.Bind<IRightRepository>().To<RightRepository>().InRequestScope();
            kernel.Bind<IReportRepository>().To<ReportRepository>().InRequestScope();
            kernel.Bind<IRequestRepository>().To<RequestRepository>().InRequestScope();
            kernel.Bind<IResourceGroupRepository>().To<ResourceGroupRepository>().InRequestScope();
            kernel.Bind<IResourceRepository>().To<ResourceRepository>().InRequestScope();
            kernel.Bind<IUserRepository>().To<UserRepository>().InRequestScope();
            kernel.Bind<ISecurityRepository>().To<SecurityRepository>().InRequestScope();
            kernel.Bind<ILdapRepository>().ToConstant(
                new LdapRepository(
                    ConfigurationManager.AppSettings["ldap_username"],
                    ConfigurationManager.AppSettings["ldap_password"]));
            // Data services
            kernel.Bind<IRightService>().To<RightService>();
            kernel.Bind<IReportService>().To<ReportService>();
            kernel.Bind<IRequestService<RequestUserModel, RequestViewModel<RequestUserModel>>>().
                To<RequestService<RequestUserModel, RequestViewModel<RequestUserModel>>>();
            kernel.Bind<IRequestAddUserService>().To<RequestAddUserService>();
            kernel.Bind<RequestDelegatePermissionsService>().To<RequestDelegatePermissionsService>();
            kernel.Bind<IRequestModifyPermissionsService>().To<RequestModifyPermissionsService>();
            kernel.Bind<IRequestRemoveUserService>().To<RequestRemoveUserService>();
            kernel.Bind<IRequestDelegatePermissionsService>().To<RequestDelegatePermissionsService>();
            kernel.Bind<IResourceGroupService>().To<ResourceGroupService>();
            kernel.Bind<IResourceService>().To<ResourceService>();
            kernel.Bind<IUserService>().To<UserService>();
            // Security services
            kernel.Bind<IReportSecurityService>().To<ReportSecurityService>();
            kernel.Bind<IResourceGroupSecurityService>().To<ResourceGroupSecurityService>();
            kernel.Bind<IResourceSecurityService>().To<ResourceSecurityService>();
            kernel.Bind<IRequestSecurityService<RequestUserModel>>().To<RequestSecurityService<RequestUserModel>>();
            kernel.Bind<IRequestSecurityService<RequestDelegatePermissionsUserModel>>().
                To<RequestSecurityService<RequestDelegatePermissionsUserModel>>();
            kernel.Bind<IUserSecurityService>().To<UserSecurityService>();
            // Email notification
            kernel.Bind<IEmailBuilder>().ToMethod(r =>
                new EmailBuilder(new MailAddress(ConfigurationManager.AppSettings["smtp_from"]),
                    kernel.Get<IRequestService<RequestUserModel, RequestViewModel<RequestUserModel>>>(),
                    kernel.Get<IRequestSecurityService<RequestUserModel>>()));
            int port;
            try
            {
                if (!int.TryParse(ConfigurationManager.AppSettings["smtp_port"], out port))
                {
                    port = 25;
                }
            }
            catch (ConfigurationErrorsException)
            {
                port = 25;
            }
            kernel.Bind<IEmailSender>().ToConstant(
                new EmailSender(port, ConfigurationManager.AppSettings["smtp_host"]));
        }        
    }
}
