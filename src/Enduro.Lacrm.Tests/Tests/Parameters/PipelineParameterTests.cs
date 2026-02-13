using System.Collections.Generic;
using Enduro.Lacrm.Parameters;
using Xunit;

namespace Enduro.Lacrm.Tests.Tests.Parameters
{
    public class PipelineParameterTests
    {
        [Fact]
        public void CreatePipelineParams_ValidData_ReturnsSuccessfulValidation()
        {
            // Arrange
            var parameters = new CreatePipelineParams(
                contactId: "contact-123",
                pipelineId: "pipeline-456",
                statusId: "status-789"
            );

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void CreatePipelineParams_NullContactId_FailsValidation()
        {
            // Arrange
            var parameters = new CreatePipelineParams(
                contactId: null!,
                pipelineId: "pipeline-456",
                statusId: "status-789"
            );

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, e => e.Parameter == "ContactId");
        }

        [Fact]
        public void CreatePipelineParams_NullPipelineId_FailsValidation()
        {
            // Arrange
            var parameters = new CreatePipelineParams(
                contactId: "contact-123",
                pipelineId: null!,
                statusId: "status-789"
            );

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, e => e.Parameter == "PipelineId");
        }

        [Fact]
        public void CreatePipelineParams_WithOptionalNote_ReturnsSuccessfulValidation()
        {
            // Arrange - Priority must be 0-3, or null
            var parameters = new CreatePipelineParams(
                contactId: "contact-123",
                pipelineId: "pipeline-456",
                statusId: "status-789",
                note: "Pipeline note"
            );

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public void UpdatePipelineItemParams_ValidData_ReturnsSuccessfulValidation()
        {
            // Arrange
            var parameters = new UpdatePipelineItemParams(
                pipelineItemId: "item-123",
                statusId: "status-789"
            );

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.True(result.Success);
        }
    }
}
