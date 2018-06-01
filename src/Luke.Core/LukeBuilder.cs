using System.Collections.Generic;
using System.Threading.Tasks;
using Luke.Core.Base;
using Luke.Core.Contracts;
using Luke.Exceptions;
using Luke.Models;
using Quartz;

namespace Luke.Core
{
    public class LukeBuilder : ILukeBuilder
    {
        private readonly ILuker _luker;
        
        public LukeBuilder(ILuker luker)
        {
            _luker = luker;
        }
        
        public async Task<IEnumerable<LukeLocationModel>> BuildAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ParameterRequiredException(nameof(path));
            }
            
            var lukeLocations = new List<LukeLocationModel>();
            IEnumerable<LukePkgModel> lukePkgModels = await _luker.ReadPkgAsync(path);
            IEnumerable<LukeLocationModel> lukeLocationModels = await _luker.LocateAsync(lukePkgModels);

            foreach (LukeLocationModel lukeLocationModel in lukeLocationModels)
            {
                bool isValidAssembly = await _luker.IsValidAssembly(lukeLocationModel);
                if (isValidAssembly)
                {
                    lukeLocations.Add(lukeLocationModel);
                    await _luker.Load(lukeLocationModel);
                }
            }

            return lukeLocations;
        }
    }
}