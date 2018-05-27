using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;

namespace Luke.Core
{
    internal class AssemblyLoader : AssemblyLoadContext
    {
        private readonly string _directoryPath;

        public AssemblyLoader(string directoryPath) =>
            _directoryPath = directoryPath;

        protected override Assembly Load(AssemblyName assemblyName)
        {
            var dependencyContext = DependencyContext.Default;
            var compilationLibraries = dependencyContext
                .CompileLibraries
                .Where(x => x.Name.Contains(assemblyName.Name))
                .ToList();
            
            if (compilationLibraries.Count > 0)
            {
                return Assembly.Load(new AssemblyName(compilationLibraries.First().Name));
            }

            var assemblyFilePath = Path.Combine(_directoryPath, assemblyName.Name);
            
            if (File.Exists(assemblyFilePath))
            {
                return LoadFromAssemblyPath(assemblyFilePath);
            }

            string dotnetSdkDirectoryPath = @"C:\Program Files\dotnet\store\x64\netcoreapp2.0";
            string runtimeStoreDirectoryPath = @"C:\Program Files\dotnet\store\x64\netcoreapp2.0";
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                dotnetSdkDirectoryPath = "/usr/local/share/dotnet/dotnet/store/x64/netcoreapp2.0";
                runtimeStoreDirectoryPath = "/usr/local/share/dotnet/dotnet/store/x64/netcoreapp2.0";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // set dotnet dir path
            }
            
            var dotnetFiles = Directory.GetFiles(dotnetSdkDirectoryPath, assemblyName.Name, SearchOption.AllDirectories);
            var sdkFiles = Directory.GetFiles(runtimeStoreDirectoryPath, assemblyName.Name, SearchOption.AllDirectories);
            var dotnetAssemblyFilePath = dotnetFiles.Concat(sdkFiles).FirstOrDefault();
            
            if (dotnetAssemblyFilePath != null)
            {
                return LoadFromAssemblyPath(dotnetAssemblyFilePath);
            }

            return Assembly.Load(assemblyName);
        }

        public Assembly Load(string assemblyName)
        {
            assemblyName = assemblyName.EndsWith(".dll") ? assemblyName : $"{assemblyName}.dll";

            return Load(new AssemblyName(assemblyName));
        }
    }
}