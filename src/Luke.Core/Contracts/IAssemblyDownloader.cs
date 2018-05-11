using System.Threading.Tasks;

namespace Luke.Core.Contracts
{
    public interface IAssemblyDownloader
    {
        Task DownloadAsync(string location, string path);
    }
}