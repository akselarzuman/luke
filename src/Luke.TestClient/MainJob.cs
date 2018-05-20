using System.Threading.Tasks;
using Luke.Core.Base;
using Luke.Core.Contracts;
using Quartz;
using Sample.DI;

namespace Sample
{
    public class MainJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            ILukeBuilder lukeBuilder = DependencyFactory.Instance.Resolve<ILukeBuilder>();
            ILukeExecutor lukeExecutor = DependencyFactory.Instance.Resolve<ILukeExecutor>();

            string path = @"C:\Users\Aksel Arzuman\Documents\GitHub\luke\src\Luke.TestClient\LukePkgConfig.json";
            
            var assembly = await lukeBuilder.BuildAsync(path);
            await lukeExecutor.ExecuteAsync(assembly);
        }
    }
}