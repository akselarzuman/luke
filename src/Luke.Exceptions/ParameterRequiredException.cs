using System;

namespace Luke.Exceptions
{
    public class ParameterRequiredException : Exception
    {
        public ParameterRequiredException(string exception) : base(exception)
        {

        }
    }
}