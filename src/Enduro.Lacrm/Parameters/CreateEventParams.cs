using System.Collections.Generic;
using System.Text.RegularExpressions;
using Enduro.Lacrm.Models;
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

        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public IEnumerable<string>? Contacts { get; set; }
        public IEnumerable<string>? Users { get; set; }
        public bool? IsAllDay { get; set; }
        public string? Location { get; set; }
        public IEnumerable<Attendee>? Attendees { get; set; }
        public bool? IsRecurring { get; set; }
        public string? RecurrenceRule { get; set; }
        public string? EndRecurrenceDate { get; set; }

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