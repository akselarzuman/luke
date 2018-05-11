using System.Threading.Tasks;
using Luke.Core.Contracts;
using Quartz;

namespace Sample
{
    public class MainJob : IJob
    {
        private readonly ISchedulerJobBuilder _schedulerJobBuilder;

        public MainJob(ISchedulerJobBuilder schedulerJobBuilder)
        {
            _schedulerJobBuilder = schedulerJobBuilder;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            string path = string.Empty;
            
            await _schedulerJobBuilder.BuildAsync(path);
        }
    }
}