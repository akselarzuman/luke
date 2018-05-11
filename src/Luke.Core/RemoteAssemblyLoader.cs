using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Luke.Core.Contracts;
using Luke.Exceptions;

namespace Luke.Core
{
    public class RemoteAssemblyLoader : IAssemblyLoader
    {
        public async Task<Assembly> LoadAsync(string fileName, string path)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ParameterRequiredException(nameof(fileName));
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ParameterRequiredException(nameof(path));
            }

            string location = $"{Directory.GetCurrentDirectory()}/{fileName}";

            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(path);
                response.EnsureSuccessStatusCode();
                using (var contentStream = await response.Content.ReadAsStreamAsync())
                {
                    using (var stream = new FileStream(location, FileMode.Create, FileAccess.Write, FileShare.None, 3145728, true))
                    {
                        await contentStream.CopyToAsync(stream);
                    }
                }
            }

            return Assembly.LoadFile(location);
        }
    }
}