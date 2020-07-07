using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class ParameterValidationResponse
    {
        public ParameterValidationResponse(bool success)
        {
            Success = success;
            Errors = new HashSet<ParameterError>();
        }

        public ParameterValidationResponse(
            bool success, 
            IEnumerable<ParameterError> errors)
        {
            Success = success;
            Errors = errors;
        }

        public ParameterValidationResponse(
            bool success,
            params ParameterError[] errors)
        {
            Success = success;
            Errors = errors;
        }

        public bool Success { get; }
        public IEnumerable<ParameterError> Errors { get; }
        
        protected bool Equals(ParameterValidationResponse other)
        {
            return Success == other.Success && Errors.Equals(other.Errors);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && 
                   Equals((ParameterValidationResponse) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Success, Errors);
        }
    }
}