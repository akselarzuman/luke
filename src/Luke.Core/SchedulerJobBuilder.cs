﻿using System;
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

        public async Task<BaseJob> BuildAsync<T>(string path) where T : BaseJob
        {
            string location = await _assemblyHelper.LoadAsync(path);
            bool isValid = _assemblyHelper.IsValidAssembly<BaseJob>(location);

            if (!isValid)
            {
                throw new InvalidAssemblyException();
            }

            Assembly assembly = Assembly.LoadFile(location);

            if (assembly == null)
            {
                throw new AssemblyNotFoundException();
            }

            Type type = typeof(T);
            Type jobsType = assembly.GetTypes().FirstOrDefault(t => t.IsClass && type.IsAssignableFrom(t));
            BaseJob baseJob = (BaseJob)Activator.CreateInstance(jobsType);

            if (baseJob == null || baseJob.LukeModel == null)
            {
                throw new InvalidAssemblyException(assembly.FullName);
            }

            return baseJob;
        }

        public async Task ExecuteAsync<T>(BaseJob baseJob) where T : BaseJob
        {
            if (baseJob == null || baseJob.LukeModel == null)
            {
                throw new InvalidAssemblyException();
            }

            LukeModel lukeModel = baseJob.LukeModel;
            IScheduler scheduler = await _schedulerFactory.GetScheduler();

            try
            {
                var job = JobBuilder.Create<T>()
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