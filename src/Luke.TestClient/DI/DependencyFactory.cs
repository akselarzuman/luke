using Luke.Core;
using Luke.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;

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
            serviceCollection.AddTransient<RemoteAssemblyLoader>();
            serviceCollection.AddTransient<LocalAssemblyLoader>();
            serviceCollection.AddTransient(factory =>
            {
                Func<string, IAssemblyLoader> accessor = key =>
                {
                    if (key.StartsWith("http"))
                    {
                        return factory.GetService<RemoteAssemblyLoader>();
                    }
                    return factory.GetService<LocalAssemblyLoader>();
                };
                return accessor;
            });
            serviceCollection.AddTransient<IAssemblyHelper, AssemblyHelper>();
            serviceCollection.AddTransient<ISchedulerJobBuilder, SchedulerJobBuilder>();
            //            serviceCollection.AddTransient<IJob, MainJob>();
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public T Resolve<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}