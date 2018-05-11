using System.Reflection;
using System.Threading.Tasks;
using Luke.Core.Contracts;
using Luke.Exceptions;

namespace Luke.Core
{
    public class LocalAssemblyLoader : IAssemblyLoader
    {
        public async Task<Assembly> LoadAsync(string fileName, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ParameterRequiredException(nameof(path));
            }

            return await Task.Run(() =>
            {
                return Assembly.ReflectionOnlyLoadFrom(path);
            });
        }
    }
}