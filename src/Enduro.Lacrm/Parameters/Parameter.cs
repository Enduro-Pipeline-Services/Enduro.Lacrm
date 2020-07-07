using System;
using System.Collections.Generic;
using System.Linq;

namespace Enduro.Lacrm.Parameters
{
    public abstract class Parameter
    {
        protected Parameter()
        {
            Validators = new List<Func<ParameterValidationResponse>>();
        }
        
        protected ICollection<Func<ParameterValidationResponse>> Validators { get; }

        protected internal ParameterValidationResponse Validate()
        {
            if (Validators == null || !Validators.Any()) 
                return new ParameterValidationResponse(true);
            
            var validators = Validators.Select(p => p.Invoke())
                .ToList();
            
            if (validators.All(v => v.Success))
                return new ParameterValidationResponse(true);

            var errors = validators.SelectMany(p => p.Errors);
            
            return new ParameterValidationResponse(false, errors);
        }
    }
}