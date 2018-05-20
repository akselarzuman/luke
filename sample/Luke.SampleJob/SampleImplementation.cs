using System;
using System.Threading.Tasks;
using Luke.SampleJob.Contracts;

namespace Luke.SampleJob
{
    public class SampleImplementation : ISampleContract
    {
        public async Task WriteAsync(string message)
        {
            await Console.Out.WriteLineAsync(message);
        }
    }
}