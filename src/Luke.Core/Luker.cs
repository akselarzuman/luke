using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Luke.Core.Base;
using Luke.Core.Contracts;
using Luke.Exceptions;
using Luke.Models;
using Newtonsoft.Json;

namespace Luke.Core
{
    public class Luker : ILuker
    {
        private readonly IAssemblyDownloader _assemblyDownloader;

        public Luker(IAssemblyDownloader assemblyDownloader)
        {
            _assemblyDownloader = assemblyDownloader;
        }

        public async Task<IEnumerable<LukePkgModel>> ReadPkgAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ParameterRequiredException(nameof(path));
            }

            if (path.StartsWith("http"))
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(path);
                    response.EnsureSuccessStatusCode();
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<LukePkgModel>>(json);
                }
            }

            using (StreamReader reader = new StreamReader(path))
            {
                var json = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<IEnumerable<LukePkgModel>>(json);
            }
        }

        public async Task<IEnumerable<LukeLocationModel>> LocateAsync(IEnumerable<LukePkgModel> lukePkgModels)
        {
            var locationModels = new List<LukeLocationModel>();
            foreach (var lukePkgModel in lukePkgModels)
            {
                string location = lukePkgModel.AssemblyLocation;

                if (lukePkgModel.AssemblyLocation.StartsWith("http"))
                {
                    location =
                        $"{Directory.GetCurrentDirectory()}/{lukePkgModel.AssemblyName.Replace(".dll", string.Empty)}";
                    await _assemblyDownloader.DownloadAsync(location, lukePkgModel.AssemblyLocation);
                }

                locationModels.Add(new LukeLocationModel
                {
                    AssemblyLocation = location,
                    AssemblyName = lukePkgModel.AssemblyName
                });
            }

            return locationModels;
        }

        public Task<bool> IsValidAssembly(LukeLocationModel lukeLocationModel)
        {
            if (lukeLocationModel == null)
            {
                throw new ParameterRequiredException(nameof(lukeLocationModel));
            }

            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if (isWindows)
            {
                // TODO : create another app domain and see if the assembly is valid
            }

            Assembly assembly =
                Assembly.LoadFile($"{lukeLocationModel.AssemblyLocation}/{lukeLocationModel.AssemblyName}");

            if (assembly == null)
            {
                throw new AssemblyNotFoundException();
            }

            bool isValidAssembly = assembly.GetTypes().Any(m => m.IsClass && typeof(LukeJob).IsAssignableFrom(m));

            return Task.FromResult(isValidAssembly);
        }

        public Task Load(LukeLocationModel lukeLocationModel)
        {
            if (lukeLocationModel == null)
            {
                throw new ParameterRequiredException(nameof(lukeLocationModel));
            }

            string[] allDllFiles = Directory.GetFiles(lukeLocationModel.AssemblyLocation, "*.dll");

            foreach (string dll in allDllFiles)
            {
                // TODO : check if assembly already loaded

                // it is loaded in IsValid method
                if (!dll.Contains(lukeLocationModel.AssemblyName))
                {
                    Assembly assembly = Assembly.LoadFile(dll.Replace("\\", "/"));

                    using (var reader = new StreamReader(dll))
                    {
                        byte[] byteArray = new byte[reader.BaseStream.Length];
                        reader.BaseStream.Read(byteArray, 0, Convert.ToInt32(reader.BaseStream.Length));
                        AppDomain.CurrentDomain.Load(byteArray);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}