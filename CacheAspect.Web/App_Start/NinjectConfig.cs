using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web;
using CacheAspect.DomainModel;
using CacheAspect.Service;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;

[assembly: WebActivator.PreApplicationStartMethod(typeof(FallenNova.Web.App_Start.NinjectConfig), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(FallenNova.Web.App_Start.NinjectConfig), "Stop")]

namespace FallenNova.Web.App_Start
{
    public static class NinjectConfig
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application.
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
        /// Creates the kernel that will manage the application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);

            return kernel;
        }

        /// <summary>
        /// Load modules and register services.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            const string constructorArgumentName = "connectionString";
            const string configurationArgumentName = "NorthwindContext";

            kernel.Bind<ICustomerService>().To<CustomerService>().InRequestScope();
            kernel.Bind<IProductService>().To<ProductService>().InRequestScope();
            kernel.Bind<ISupplierService>().To<SupplierService>().InRequestScope();

            // This approach allows a single transaction to support multiple operations across multiple repositories.
            kernel.Bind<NorthwindContext>().ToSelf().InRequestScope().WithConstructorArgument(constructorArgumentName, ConfigurationManager.ConnectionStrings[configurationArgumentName].ConnectionString);
            kernel.Bind<IUnitOfWork>().ToMethod(ctx => ctx.Kernel.Get<NorthwindContext>());
            kernel.Bind<DbContext>().ToMethod(ctx => ctx.Kernel.Get<NorthwindContext>());

            var modules = new List<INinjectModule>
                {
                    new ServiceModule()
                };
            kernel.Load(modules);
        }
    }
}
