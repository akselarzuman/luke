using Luke.Core.Base;
using Quartz;
using System.Threading.Tasks;
using Luke.Models;
using Luke.SampleJob.Autofac.Contracts;

namespace Luke.SampleJob.Autofac
{
    public class TestJob : LukeJob
    {
        public override LukeModel LukeModel => new LukeModel
        {
            IdentityName = "test_job_autofac",
            IdentityGroup = "test_group_autofac",
            IdentityTrigger = "test_trigger_autofac",
            Interval = 10,
            IntervalType = IntervalType.SEC,
            IsRepeatForever = true,
            ScheduleType = ScheduleType.SIMPLE
        };

        public override Task RegisterDependencies()
        {
            DependencyFactory.Instance.RegisterDependencies();
            return Task.CompletedTask;
        }

        public override Task Execute(IJobExecutionContext context)
        {
            ISampleContract sampleContract = DependencyFactory.Instance.Resolve<ISampleContract>();
            sampleContract.WriteAsync("Test message from Luke.SampleJob.Autofac");
            
            return Task.CompletedTask;
        }
    }
}