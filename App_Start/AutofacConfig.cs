using Autofac;
using Autofac.Integration.Mvc;
using SampleWebProject.Infrastructure;
using System;
using System.Web.Mvc;
using SampleWebProject.Infrastructure.Interface;

namespace SampleWebProject.App_Start {
    public class AutofacConfig {
        private static readonly AutofacConfig _instance;
        public IContainer Container { get; private set; }

        static AutofacConfig() {
            _instance = new AutofacConfig();
            _instance.RegisterTypes();
        }

        private void RegisterTypes() {
            var builder = new ContainerBuilder();

            // Register dependencies in controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Get assemblies
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            builder.RegisterType<UnitOfWork>()
                    .As<IUnitOfWork>()
                    .InstancePerLifetimeScope();

         builder.RegisterAssemblyTypes(assemblies)
                                .Where(t => t.Name.EndsWith("Service"))
                                .AsImplementedInterfaces()
                                .InstancePerRequest();

            builder.RegisterAssemblyTypes(assemblies)
                                .Where(t => t.Name.EndsWith("Repository"))
                                .AsImplementedInterfaces()
                                .InstancePerRequest();

            builder.RegisterAssemblyTypes(assemblies)
                                .Where(t => t.Name.EndsWith("Validator"))
                                .AsImplementedInterfaces()
                                .InstancePerRequest();

            Container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
        }

        public TType Resolve<TType>() {
            return Container.Resolve<TType>();
        }

        public static void Initialize() {
        }

        public static AutofacConfig Instance {
            get {
                return _instance;
            }
        }
    }
}