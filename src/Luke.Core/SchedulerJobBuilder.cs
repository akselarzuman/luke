using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Luke.Core.Base;
using Luke.Core.Contracts;
using Luke.Exceptions;
using Luke.Models;
using Quartz;

namespace Luke.Core
{
    public class SchedulerJobBuilder : ISchedulerJobBuilder
    {
        private readonly IAssemblyHelper _assemblyHelper;
        private readonly ISchedulerFactory _schedulerFactory;

        public SchedulerJobBuilder(IAssemblyHelper assemblyHelper, ISchedulerFactory schedulerFactory)
        {
            _assemblyHelper = assemblyHelper;
            _schedulerFactory = schedulerFactory;
        }

        public async Task<LukeJob> BuildAsync(string path)
        {
            string location = await _assemblyHelper.LoadAsync(path);
            bool isValid = _assemblyHelper.IsValidAssembly<LukeJob>(location);

            if (!isValid)
            {
                throw new InvalidAssemblyException();
            }

            Assembly assembly = Assembly.LoadFile(location);

            if (assembly == null)
            {
                throw new AssemblyNotFoundException();
            }

            Type jobsType = assembly.GetTypes().FirstOrDefault(t => t.IsClass && typeof(LukeJob).IsAssignableFrom(t));
            LukeJob lukeJob = (LukeJob)Activator.CreateInstance(jobsType);

            if (lukeJob == null || lukeJob.LukeModel == null)
            {
                throw new InvalidAssemblyException(assembly.FullName);
            }

            return lukeJob;
        }

        public async Task ExecuteAsync(LukeJob lukeJob)
        {
            if (lukeJob == null || lukeJob.LukeModel == null)
            {
                throw new InvalidAssemblyException();
            }

            LukeModel lukeModel = lukeJob.LukeModel;
            IScheduler scheduler = await _schedulerFactory.GetScheduler();

            try
            {
                var job = JobBuilder.Create(lukeJob.GetType())
                    .WithIdentity(lukeModel.IdentityName, lukeModel.IdentityGroup)
                    .Build();

                TriggerBuilder triggerBuilder = TriggerBuilder.Create()
                    .WithIdentity(lukeModel.IdentityTrigger, lukeModel.IdentityGroup)
                    .StartNow();

                if (lukeModel.ScheduleType == ScheduleType.SIMPLE)
                {
                    triggerBuilder.WithSimpleSchedule(x =>
                    {
                        switch (lukeModel.IntervalType)
                        {
                            case IntervalType.SEC:
                                x.WithIntervalInSeconds(lukeModel.Interval);
                                break;
                            case IntervalType.MIN:
                                x.WithIntervalInMinutes(lukeModel.Interval);
                                break;
                            case IntervalType.HOUR:
                            default:
                                x.WithIntervalInHours(lukeModel.Interval);
                                break;
                        }

                        if (lukeModel.IsRepeatForever)
                        {
                            x.RepeatForever();
                        }
                    });
                }

                ITrigger trigger = triggerBuilder.Build();

                await scheduler.ScheduleJob(job, trigger);
            }
            catch (SchedulerException se)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(se);
            }

        }
    }
}