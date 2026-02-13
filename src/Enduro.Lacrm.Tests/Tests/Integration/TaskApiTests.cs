using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using Moq;
using Moq.Protected;
using Xunit;

namespace Enduro.Lacrm.Tests.Tests.Integration
{
    /// <summary>
    /// Integration tests for Task API operations.
    /// These tests demonstrate how to call Task APIs with mocked HttpClient.
    /// </summary>
    public class TaskApiTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly LacrmClient _lacrmClient;

        public TaskApiTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            
            var options = new Options
            {
                ApiUrl = "https://api.lessannoyingcrm.com",
                ApiToken = "test-token",
                UserCode = "test-user"
            };
            
            _lacrmClient = new LacrmClient(_httpClient, options);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_ValidParameters_ReturnsSuccessResponse()
        {
            // Arrange
            var responseJson = @"{
                ""Success"": true,
                ""TaskId"": ""task-12345""
            }";

            _mockHttpMessageHandler.Protected()
                .Setup<System.Threading.Tasks.Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseJson)
                });

            // Act
            var response = await _lacrmClient.CreateTask(
                contactId: "contact-123",
                dueDate: "2024-12-31",
                name: "Complete Project",
                description: "Finish the integration tests"
            );

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal("task-12345", response.TaskId);
        }

        /// <summary>
        /// Example usage pattern for real-world scenarios:
        /// 
        /// // Initialize the client with real credentials
        /// var options = new Options
        /// {
        ///     ApiUrl = "https://api.lessannoyingcrm.com",
        ///     ApiToken = "your-api-token",
        ///     UserCode = "your-user-code"
        /// };
        /// var client = new LacrmClient(new HttpClient(), options);
        /// 
        /// // Create a new task
        /// var createResponse = await client.CreateTask(
        ///     contactId: "contact-123",
        ///     dueDate: "2024-12-31",
        ///     name: "Follow up with client",
        ///     description: "Discuss project timeline"
        /// );
        /// 
        /// // Edit a task
        /// var editParams = new EditTaskParams(createResponse.TaskId)
        /// {
        ///     IsCompleted = true
        /// };
        /// await client.EditTask(editParams);
        /// 
        /// // Get all tasks for a contact
        /// var tasks = await client.GetTasksAttachedToContact("contact-123");
        /// </summary>
    }
}
