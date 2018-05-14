using System;
using Luke.Core.Base;
using Quartz;
using System.Threading.Tasks;
using Luke.Models;

namespace Luke.SampleJob
{
    public class TestJob : BaseJob
    {
        public override LukeModel LukeModel => new LukeModel
        {
            IdentityName = "test_job",
            IdentityGroup = "test_group",
            IdentityTrigger = "test_trigger",
            Interval = 10,
            IntervalType = IntervalType.SEC,
            IsRepeatForever = true,
            ScheduleType = ScheduleType.SIMPLE
        };

        public override Task RegisterDependencies()
        {
            return Task.CompletedTask;
        }

        public override Task Execute(IJobExecutionContext context)
        {
            System.Console.WriteLine(DateTime.Now);
            return Task.CompletedTask;
        }
    }
}
