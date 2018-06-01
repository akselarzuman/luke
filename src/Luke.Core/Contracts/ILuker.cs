using System.Collections.Generic;
using System.Threading.Tasks;
using Luke.Models;

namespace Luke.Core.Contracts
{
    public interface ILuker
    {
        Task<IEnumerable<LukePkgModel>> ReadPkgAsync(string path);
        Task<IEnumerable<LukeLocationModel>> LocateAsync(IEnumerable<LukePkgModel> lukePkgModels);
        Task<bool> IsValidAssembly(LukeLocationModel lukeLocationModel);
        Task Load(LukeLocationModel lukeLocationModel);
    }
}