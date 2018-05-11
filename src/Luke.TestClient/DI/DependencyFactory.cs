using Luke.Core;
using Luke.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;

namespace Sample.DI
{
    public class DependencyFactory
    {
        public static readonly DependencyFactory Instance = new DependencyFactory();

        private DependencyFactory()
        {
        }

        private ServiceProvider _serviceProvider;

        public void RegisterDependencies()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IAssemblyDownloader, AssemblyDownloader>();
            serviceCollection.AddTransient<IAssemblyHelper, AssemblyHelper>();
            serviceCollection.AddTransient<ISchedulerJobBuilder, SchedulerJobBuilder>();
            serviceCollection.AddTransient<ISchedulerFactory, StdSchedulerFactory>();
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public T Resolve<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}