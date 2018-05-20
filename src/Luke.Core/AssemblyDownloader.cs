using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Luke.Core.Contracts;
using Luke.Exceptions;

namespace Luke.Core
{
    public class AssemblyDownloader : IAssemblyDownloader
    {
        public async Task DownloadAsync(string location, string url)
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new ParameterRequiredException(nameof(location));
            }

            if (string.IsNullOrEmpty(url))
            {
                throw new ParameterRequiredException(nameof(url));
            }
            
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                using (var contentStream = await response.Content.ReadAsStreamAsync())
                {
                    using (var stream = new FileStream(location, FileMode.Create, FileAccess.Write, FileShare.None, 3145728, true))
                    {
                        await contentStream.CopyToAsync(stream);
                    }
                }
            }
        }
    }
}