using Luke.Core.Base;
using System.Reflection;
using System.Threading.Tasks;

namespace Luke.Core.Contracts
{
    public interface ISchedulerJobBuilder
    {
        Task<Assembly> BuildAsync(string path);
        Task ExecuteAsync<T>(Assembly assembly) where T : BaseJob;
    }
}