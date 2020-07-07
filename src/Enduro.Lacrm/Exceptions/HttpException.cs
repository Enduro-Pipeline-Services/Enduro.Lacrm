using System;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Exceptions
{
    [PublicAPI]
    public class HttpException : Exception
    {
        public HttpException() {}
        public HttpException(string message) : base(message) {}
        public  HttpException(string message, Exception exception) 
            : base(message, exception) {}
    }
}