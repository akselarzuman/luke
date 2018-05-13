using System;
using Quartz;
using Luke.Sample.DI;

namespace Luke.Sample
{
    public class Program
    {
        private static IScheduler _scheduler;

        public static void Main(string[] args)
        {
            DependencyFactory.Instance.RegisterDependencies();
            ISchedulerFactory schedulerFactory = DependencyFactory.Instance.Resolve<ISchedulerFactory>();
            _scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            _scheduler.Start();

            ExecuteIterableJob();

            Console.ReadLine();
        }

        private static void ExecuteIterableJob()
        {
            try
            {
                var job = JobBuilder.Create<MainJob>()
                    .WithIdentity("luke", "luke_group")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("luke_trigger", "luke_group")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInHours(1)
                        .RepeatForever())
                    .Build();

                _scheduler.ScheduleJob(job, trigger);
            }
            catch (SchedulerException se)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(se);
            }
        }
    }
}