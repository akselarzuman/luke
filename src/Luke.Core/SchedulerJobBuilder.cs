using System;
using System.Reflection;
using System.Threading.Tasks;
using Luke.Core.Base;
using Luke.Core.Contracts;
using Luke.Exceptions;

namespace Luke.Core
{
    public class SchedulerJobBuilder : ISchedulerJobBuilder
    {
        private readonly IAssemblyHelper _assemblyHelper;

        public SchedulerJobBuilder(IAssemblyHelper assemblyHelper)
        {
            _assemblyHelper = assemblyHelper;
        }

        public async Task<Assembly> BuildAsync(string path)
        {
            Assembly assembly = await _assemblyHelper.LoadAsync(path);
            bool isValid = _assemblyHelper.IsValidAssembly<BaseJob>(assembly);

            if (!isValid)
            {
                throw new InvalidAssemblyException();
            }

            return assembly;
        }

        public async Task ExecuteAsync<T>(Assembly assembly) where T : BaseJob
        {
            throw new NotImplementedException();
        }
    }
}