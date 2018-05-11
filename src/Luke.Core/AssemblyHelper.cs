using System;
using System.IO;
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
        private readonly IAssemblyDownloader _assemblyDownloader;

        public AssemblyHelper(IAssemblyDownloader assemblyDownloader)
        {
            _assemblyDownloader = assemblyDownloader;
        }

        public async Task<string> LoadAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ParameterRequiredException(nameof(path));
            }

            int lastIndex = path.LastIndexOf("/", StringComparison.Ordinal) + 1;
            string filename = path.Substring(lastIndex, path.Length - lastIndex);
            string location = path;

            if (path.StartsWith("http"))
            {
                location = $"{Directory.GetCurrentDirectory()}/{location}";

                await _assemblyDownloader.DownloadAsync(location, path);
            }

            return location;
        }

        public bool IsValidAssembly<T>(string location) where T : BaseJob
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new ParameterRequiredException(nameof(location));
            }

            Assembly assembly = Assembly.LoadFile(location);

            if (assembly == null)
            {
                throw new AssemblyNotFoundException();
            }

            return assembly.GetTypes().Any(m => m.IsClass && typeof(T).IsAssignableFrom(m));
        }
    }
}