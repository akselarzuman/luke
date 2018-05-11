using System.Threading.Tasks;
using Luke.Models;
using Quartz;

namespace Luke.Core.Base
{
    public abstract class BaseJob : IJob
    {
        public LukeModel LukeModel { get; set; }

        public abstract Task Execute(IJobExecutionContext context);
    }
}