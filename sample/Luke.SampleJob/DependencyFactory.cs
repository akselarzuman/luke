using Luke.SampleJob.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Luke.SampleJob
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
            serviceCollection.AddTransient<ISampleContract, SampleImplementation>();
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public T Resolve<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}