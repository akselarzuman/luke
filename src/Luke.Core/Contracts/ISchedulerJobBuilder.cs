using Luke.Core.Base;
using System.Threading.Tasks;

namespace Luke.Core.Contracts
{
    public interface ISchedulerJobBuilder
    {
        Task<BaseJob> BuildAsync<T>(string path) where T : BaseJob;
        Task ExecuteAsync<T>(BaseJob baseJob) where T : BaseJob;
    }
}