﻿using System.Threading.Tasks;
using Luke.Models;
using Quartz;

namespace Luke.Core.Base
{
    public abstract class BaseJob : IJob
    {
        public abstract LukeModel LukeModel { get; }

        public abstract Task Execute(IJobExecutionContext context);
    }
}