using Enduro.Lacrm.Parameters;
using Xunit;

namespace Enduro.Lacrm.Tests.Tests.Parameters
{
    public class TaskParameterTests
    {
        [Fact]
        public void CreateTaskParams_ValidData_ReturnsSuccessfulValidation()
        {
            // Arrange
            var parameters = new CreateTaskParams(
                contactId: "12345",
                dueDate: "2024-12-31",
                name: "Test Task",
                description: "Test Description"
            );

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void CreateTaskParams_NullContactId_FailsValidation()
        {
            // Arrange
            var parameters = new CreateTaskParams(
                contactId: null!,
                dueDate: "2024-12-31",
                name: "Test Task",
                description: "Test Description"
            );

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, e => e.Parameter == "ContactId");
        }

        [Fact]
        public void EditTaskParams_ValidTaskId_ReturnsSuccessfulValidation()
        {
            // Arrange
            var parameters = new EditTaskParams("task-123")
            {
                Name = "Updated Task Name",
                DueDate = "2024-12-31"
            };

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void EditTaskParams_NullTaskId_FailsValidation()
        {
            // Arrange
            var parameters = new EditTaskParams(null!)
            {
                Name = "Updated Task Name"
            };

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, e => e.Parameter == "TaskId");
        }

        [Fact]
        public void EditTaskParams_AllOptionalFields_ValidatesSuccessfully()
        {
            // Arrange
            var parameters = new EditTaskParams("task-123")
            {
                Name = "Task Name",
                DueDate = "2024-12-31",
                AssignedTo = "user-123",
                CalendarId = "calendar-456",
                Description = "Task description",
                ContactId = "contact-789",
                IsCompleted = true
            };

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Task Name", parameters.Name);
            Assert.Equal("2024-12-31", parameters.DueDate);
            Assert.Equal("user-123", parameters.AssignedTo);
            Assert.Equal("calendar-456", parameters.CalendarId);
            Assert.Equal("Task description", parameters.Description);
            Assert.Equal("contact-789", parameters.ContactId);
            Assert.True(parameters.IsCompleted);
        }
    }
}
