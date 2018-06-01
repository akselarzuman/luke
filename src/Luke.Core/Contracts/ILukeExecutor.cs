using System.Collections.Generic;
using System.Threading.Tasks;
using Luke.Models;

namespace Luke.Core.Contracts
{
    public interface ILukeExecutor
    {
        Task ExecuteAsync(IEnumerable<LukeLocationModel> lukeLocationModels);
    }
}