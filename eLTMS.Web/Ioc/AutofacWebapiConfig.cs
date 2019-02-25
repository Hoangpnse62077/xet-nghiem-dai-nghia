using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess;
using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using eLTMS.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace eLTMS.Web.Ioc
{
    public class AutofacWebapiConfig
    {
        public static IContainer Container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }


        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }


        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //builder.RegisterType<DatabaseConnectionSettings>().As<IDatabaseConnectionSettings>();
            builder.RegisterType<RepositoryHelper>().As<IRepositoryHelper>();

            builder.RegisterType<DbContextTransactionProxy>().As<IDbContextTransactionProxy>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<FinalProjectContext>().AsSelf().InstancePerRequest();
            
            // Repositories
            builder.RegisterAssemblyTypes(typeof(AccountRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            // Services
            builder.RegisterAssemblyTypes(typeof(AccountService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            Container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
            return Container;

        }
    }
}