using System.Collections.Generic;
using System.Threading.Tasks;
using Luke.Models;

namespace Luke.Core.Contracts
{
    public interface ILukeBuilder
    {
        Task<IEnumerable<LukeLocationModel>> BuildAsync(string path);
    }
}