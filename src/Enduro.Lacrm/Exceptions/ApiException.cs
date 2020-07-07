using System;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Exceptions
{
    [PublicAPI]
    public class ApiException : Exception
    {
        public ApiException() {}
        public ApiException(string message) : base(message) {}
        public  ApiException(string message, Exception exception) 
            : base(message, exception) {}
    }
}