using System.Collections.Generic;
using Enduro.Lacrm.Parameters;
using Xunit;

namespace Enduro.Lacrm.Tests.Tests.Parameters
{
    public class EventParameterTests
    {
        [Fact]
        public void CreateEventParams_ValidData_ReturnsSuccessfulValidation()
        {
            // Arrange
            var parameters = new CreateEventParams(
                date: "2024-12-31",
                startTime: "09:00",
                endTime: "10:00",
                name: "Test Event"
            );

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void CreateEventParams_InvalidDateFormat_FailsValidation()
        {
            // Arrange
            var parameters = new CreateEventParams(
                date: "12/31/2024",
                startTime: "09:00",
                endTime: "10:00",
                name: "Test Event"
            );

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, e => e.Parameter == "Date");
        }

        [Fact]
        public void CreateEventParams_InvalidStartTimeFormat_FailsValidation()
        {
            // Arrange
            var parameters = new CreateEventParams(
                date: "2024-12-31",
                startTime: "9am",
                endTime: "10:00",
                name: "Test Event"
            );

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, e => e.Parameter == "StartTime");
        }

        [Fact]
        public void CreateEventParams_InvalidEndTimeFormat_FailsValidation()
        {
            // Arrange
            var parameters = new CreateEventParams(
                date: "2024-12-31",
                startTime: "09:00",
                endTime: "10pm",
                name: "Test Event"
            );

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, e => e.Parameter == "EndTime");
        }

        [Fact]
        public void CreateEventParams_WithContactsAndUsers_ReturnsSuccessfulValidation()
        {
            // Arrange
            var contacts = new List<string> { "contact-1", "contact-2" };
            var users = new List<string> { "user-1", "user-2" };
            
            var parameters = new CreateEventParams(
                date: "2024-12-31",
                startTime: "09:00",
                endTime: "10:00",
                name: "Test Event",
                description: "Event description",
                contacts: contacts,
                users: users
            );

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Event description", parameters.Description);
        }
    }
}
