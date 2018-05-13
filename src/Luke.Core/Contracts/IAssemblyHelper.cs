using System.Threading.Tasks;

namespace Luke.Core.Contracts
{
    public interface IAssemblyHelper
    {
        Task<string> LoadAsync();
        Task<string> LoadAsync(string path);
        Task<bool> IsValidAssembly();
        Task<bool> IsValidAssembly(string location);
    }
}