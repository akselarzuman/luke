using System;
using System.Linq;
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
            string location = await _assemblyHelper.LoadAsync(path);
            bool isValid = _assemblyHelper.IsValidAssembly<BaseJob>(location);

            if (!isValid)
            {
                throw new InvalidAssemblyException();
            }

            return Assembly.LoadFile(location);
        }

        public async Task ExecuteAsync<T>(Assembly assembly) where T : BaseJob
        {
            if (assembly == null)
            {
                throw new AssemblyNotFoundException();
            }

            Type type = typeof(T);
            Type jobsType = assembly.GetTypes().FirstOrDefault(t => t.IsClass && type.IsAssignableFrom(t));


        }
    }
}