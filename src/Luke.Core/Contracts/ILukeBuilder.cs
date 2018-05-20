using System.Threading.Tasks;
using Luke.Core.Base;

namespace Luke.Core.Contracts
{
    public interface ILukeBuilder
    {
        Task BuildAsync(string path);
    }
}