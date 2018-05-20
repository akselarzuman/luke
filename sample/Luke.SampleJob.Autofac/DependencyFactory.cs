using Autofac;
using Luke.SampleJob.Autofac.Contracts;

namespace Luke.SampleJob.Autofac
{
    public class DependencyFactory
    {
        public static readonly DependencyFactory Instance = new DependencyFactory();

        private DependencyFactory()
        {
        }

        private IContainer _container;

        public void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SampleImplementation>().As<ISampleContract>();
            _container = builder.Build();
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}