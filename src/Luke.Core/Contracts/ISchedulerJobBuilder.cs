using Luke.Core.Base;
using System.Threading.Tasks;

namespace Luke.Core.Contracts
{
    public interface ISchedulerJobBuilder
    {
        Task<LukeJob> BuildAsync(string path);
        Task ExecuteAsync(LukeJob lukeJob);
    }
}