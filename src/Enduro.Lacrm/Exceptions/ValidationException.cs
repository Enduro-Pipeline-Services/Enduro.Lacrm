using System;
using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Exceptions
{
    [PublicAPI]
    public class ValidationException: Exception
    {
        public ParameterValidationResponse? Response { get; }
        public ValidationException() {}
        public ValidationException(string message) : base(message) {}
        public  ValidationException(string message, Exception exception) 
            : base(message, exception) {}

        public ValidationException(ParameterValidationResponse response) :
            base("One or more validation exceptions occurred")
        {
            Response = response;
        }
    }
}