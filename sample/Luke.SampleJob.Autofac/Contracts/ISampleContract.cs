using System.Threading.Tasks;

namespace Luke.SampleJob.Autofac.Contracts
{
    public interface ISampleContract
    {
        Task WriteAsync(string message);
    }
}