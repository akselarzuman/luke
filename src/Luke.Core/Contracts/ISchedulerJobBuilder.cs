using Luke.Core.Base;
using System.Threading.Tasks;

namespace Luke.Core.Contracts
{
    public interface ISchedulerJobBuilder
    {
        Task<BaseJob> BuildAsync(string path);
        Task ExecuteAsync(BaseJob baseJob);
    }
}