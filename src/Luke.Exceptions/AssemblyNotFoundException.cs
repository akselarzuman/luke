using System;

namespace Luke.Exceptions
{
    public class AssemblyNotFoundException : Exception
    {
        public AssemblyNotFoundException()
        {

        }

        public AssemblyNotFoundException(string exception) : base(exception)
        {

        }
    }
}