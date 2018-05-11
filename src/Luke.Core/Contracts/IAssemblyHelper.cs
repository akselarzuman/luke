using Luke.Core.Base;
using System.Reflection;
using System.Threading.Tasks;

namespace Luke.Core.Contracts
{
    public interface IAssemblyHelper
    {
        Task<string> LoadAsync(string path);
        bool IsValidAssembly<T>(string location) where T : BaseJob;
    }
}