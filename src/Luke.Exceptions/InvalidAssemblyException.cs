using System;

namespace Luke.Exceptions
{
    public class InvalidAssemblyException : Exception
    {
        public InvalidAssemblyException()
        {

        }

        public InvalidAssemblyException(string exception) : base(exception)
        {

        }
    }
}