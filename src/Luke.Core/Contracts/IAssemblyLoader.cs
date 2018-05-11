using System.Reflection;
using System.Threading.Tasks;

namespace Luke.Core.Contracts
{
    public interface IAssemblyLoader
    {
        Task<Assembly> LoadAsync(string fileName, string path);
    }
}