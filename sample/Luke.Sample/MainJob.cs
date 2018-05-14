using System.Threading.Tasks;
using Luke.Core.Contracts;
using Quartz;
using Luke.Sample.DI;
using System.IO;

namespace Luke.Sample
{
    public class MainJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            ISchedulerJobBuilder schedulerJobBuilder = DependencyFactory.Instance.Resolve<ISchedulerJobBuilder>();

            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\Luke.SampleJob\bin\Debug\netstandard2.0\Luke.SampleJob.dll"));

            var assembly = await schedulerJobBuilder.BuildAsync(path);
            assembly.RegisterDependencies().GetAwaiter().GetResult();
            await schedulerJobBuilder.ExecuteAsync(assembly);
        }
    }
}