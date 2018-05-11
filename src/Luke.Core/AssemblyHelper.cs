using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Luke.Core.Base;
using Luke.Core.Contracts;
using Luke.Exceptions;

namespace Luke.Core
{
    public class AssemblyHelper : IAssemblyHelper
    {
        private readonly Func<string, IAssemblyLoader> _assemblyLoader;

        public AssemblyHelper(Func<string, IAssemblyLoader> assemblyLoader)
        {
            _assemblyLoader = assemblyLoader;
        }

        public async Task<Assembly> LoadAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ParameterRequiredException(nameof(path));
            }

            int lastIndex = path.LastIndexOf("/", StringComparison.Ordinal) + 1;
            string filename = path.Substring(lastIndex, path.Length - lastIndex);

            return await _assemblyLoader(path).LoadAsync(filename, path);
        }

        public bool IsValidAssembly<T>(Assembly assembly) where T : BaseJob
        {
            if (assembly == null)
            {
                throw new AssemblyNotFoundException();
            }

            foreach (var typeInfo in assembly.DefinedTypes)
            {
                if (typeInfo.ImplementedInterfaces.Contains(typeof(T)))
                {
                    return true;
                }
            }

            return false;
        }

        public async Task RemoveAssemblyAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ParameterRequiredException(nameof(path));
            }

            throw new NotImplementedException();
        }
    }
}