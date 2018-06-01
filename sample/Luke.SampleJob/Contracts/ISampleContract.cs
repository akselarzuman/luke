using System.Threading.Tasks;

namespace Luke.SampleJob.Contracts
{
    public interface ISampleContract
    {
        Task WriteAsync(string message);
    }
}