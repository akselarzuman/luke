using System.Threading.Tasks;
using Luke.Core.Base;
using Luke.Core.Contracts;
using Quartz;
using Luke.Sample.DI;

namespace Luke.Sample
{
    public class MainJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            ISchedulerJobBuilder schedulerJobBuilder = DependencyFactory.Instance.Resolve<ISchedulerJobBuilder>();

            string path = @"..\..\Luke.SampleJob\bin\Debug\netstandard2.0\Luke.SampleJob.dll";
            
            var assembly = await schedulerJobBuilder.BuildAsync(path);
            await schedulerJobBuilder.ExecuteAsync(assembly);
        }
    }
}