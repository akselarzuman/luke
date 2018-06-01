using System.Threading.Tasks;
using Luke.Core.Contracts;
using Quartz;
using Luke.Sample.DI;
using System.IO;
using Luke.Models;
using System.Collections.Generic;

namespace Luke.Sample
{
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
}