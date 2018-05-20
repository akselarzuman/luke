using System;
using System.Threading.Tasks;
using Luke.SampleJob.Autofac.Contracts;

namespace Luke.SampleJob.Autofac
{
    public class SampleImplementation : ISampleContract
    {
        public async Task WriteAsync(string message)
        {
            await Console.Out.WriteLineAsync(message);
        }
    }
}