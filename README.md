# luke
Luke is scheduler job infrastructure to load and run assemblies easily.

Currently supports only .Net Core 2.1 runtime.

## Installation
[![NuGet](https://img.shields.io/nuget/v/Luke.svg)](https://www.nuget.org/packages/Luke)

To install Luke, run the following command in the Package Manager Console

```
Install-Package Luke -Version 1.0.2
```

or in terminal

```
dotnet add package Luke --version 1.0.2
```

# Usage

## MainJob

Your main job is to check a json file and to import the scheduler jobs implementing LukeJob abstract class into your AppDomain and run them.

Your main project must reference Luke as well and implement it as following

```csharp
public class MainJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            ILukeBuilder lukeBuilder = DependencyFactory.Instance.Resolve<ILukeBuilder>();
            ILukeExecutor lukeExecutor = DependencyFactory.Instance.Resolve<ILukeExecutor>();

            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "LukePkg.json"));

            IEnumerable<LukeLocationModel> lukeLocationModels = await lukeBuilder.BuildAsync(path);
            await lukeExecutor.ExecuteAsync(lukeLocationModels);
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine(ex);
            throw;
        }
    }
}
```

Here you can use an IOC container or you can create the instances. It depends on you.

## Json File

Your main project must have a json file and it must be an array having AssemblyName and AssemblyLocation tags. You can name it as "LukePkg.json"

```csharp
[
  {
    "AssemblyName": "Luke.SampleJob.dll",
    "AssemblyLocation": "C:/Users/AkseArzuman/Desktop/Github/luke/sample/Luke.SampleJob/bin/Release/netstandard2.0/publish"
  }
]
```

## Scheduler Job

Your scheduler job must implement LukeJob. If you are using a IOC container you must register your dependencies in RegisterDependencies method.

Please see : https://github.com/akselarzuman/luke/tree/master/sample