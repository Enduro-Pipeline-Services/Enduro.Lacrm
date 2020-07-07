using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class CreateEventParams : Parameter
    {
        public CreateEventParams(string date, 
            string startTime, 
            string endTime, 
            string name,
            string? description = null,
            IEnumerable<string>? contacts = null,
            IEnumerable<string>? users = null)
        {
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Name = name;
            Description = description;
            Contacts = contacts;
            Users = users;

            Validators.Add(ValidateDate);
            Validators.Add(ValidateStartTime);
            Validators.Add(ValidateEndTime);
        }

        public string Date { get; }
        public string StartTime { get; }
        public string EndTime { get; }
        public string Name { get; }
        public string? Description { get; }
        public IEnumerable<string>? Contacts { get; }
        public IEnumerable<string>? Users { get; }

        protected virtual ParameterValidationResponse ValidateDate()
        {
            var valid = new Regex("^\\d{4}\\-(0[1-9]|1[012])\\-(0[1-9]|[12][0-9]|3[01])$")
                .IsMatch(Date);

            if (valid)
                return new ParameterValidationResponse(true);

            return new ParameterValidationResponse(false,
                new ParameterError(nameof(Date), "Date must be in YYYY-MM-DD format."));
        }

        protected virtual ParameterValidationResponse ValidateStartTime()
        {
            var valid = new Regex("^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$")
                .IsMatch(StartTime);

            if (valid)
                return new ParameterValidationResponse(true);

            return new ParameterValidationResponse(false,
                new ParameterError(nameof(StartTime), "StartTime must be in HH:MM format."));
        }

        protected virtual ParameterValidationResponse ValidateEndTime()
        {
            var valid = new Regex("^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$")
                .IsMatch(EndTime);

            if (valid)
                return new ParameterValidationResponse(true);

            return new ParameterValidationResponse(false,
                new ParameterError(nameof(EndTime), "EndTime must be in HH:MM format."));
        }
    }
}