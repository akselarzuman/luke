using System;
using System.Collections.Generic;
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
    public class LukeExecutor : ILukeExecutor
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public LukeExecutor(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        public async Task ExecuteAsync(IEnumerable<LukeLocationModel> lukeLocationModels)
        {
            foreach (LukeLocationModel lukeLocationModel in lukeLocationModels)
            {
                AssemblyLoader assemblyLoader = new AssemblyLoader(lukeLocationModel.AssemblyLocation);
                Assembly assembly = assemblyLoader.Load(lukeLocationModel.AssemblyName);
                Type jobsType = assembly.GetTypes().FirstOrDefault(t => t.IsClass && typeof(LukeJob).IsAssignableFrom(t));

                if (jobsType == null)
                {
                    throw new InvalidAssemblyException(assembly.FullName);
                }

                LukeJob lukeJob = (LukeJob)Activator.CreateInstance(jobsType);

                if (lukeJob.LukeModel == null)
                {
                    throw new InvalidAssemblyException(assembly.FullName);
                }

                await lukeJob.RegisterDependencies();

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
}