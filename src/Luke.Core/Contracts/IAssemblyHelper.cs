using Luke.Core.Base;
using System.Reflection;
using System.Threading.Tasks;

namespace Luke.Core.Contracts
{
    public interface IAssemblyHelper
    {
        Task<Assembly> LoadAsync(string path);
        bool IsValidAssembly<T>(Assembly assembly) where T : BaseJob;
        Task RemoveAssemblyAsync(string path);
    }
}