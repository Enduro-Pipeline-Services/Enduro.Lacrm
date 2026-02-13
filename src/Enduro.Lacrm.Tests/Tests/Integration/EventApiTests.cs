using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using Moq;
using Moq.Protected;
using Xunit;

namespace Enduro.Lacrm.Tests.Tests.Integration
{
    /// <summary>
    /// Integration tests for Event API operations.
    /// These tests demonstrate how to call Event APIs with mocked HttpClient.
    /// </summary>
    public class EventApiTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly LacrmClient _lacrmClient;

        public EventApiTests()
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
        public async System.Threading.Tasks.Task CreateEvent_ValidParameters_ReturnsSuccessResponse()
        {
            // Arrange
            var responseJson = @"{
                ""Success"": true,
                ""EventId"": ""event-12345""
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
            var response = await _lacrmClient.CreateEvent(
                date: "2024-12-31",
                startTime: "14:00",
                endTime: "15:00",
                name: "Client Meeting",
                description: "Discuss Q1 2025 goals"
            );

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal("event-12345", response.EventId);
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
        /// // Create a new event
        /// var createResponse = await client.CreateEvent(
        ///     date: "2024-12-31",
        ///     startTime: "14:00",
        ///     endTime: "15:00",
        ///     name: "Client Meeting",
        ///     description: "Quarterly review"
        /// );
        /// 
        /// // Edit an event
        /// var editParams = new EditEventParams(createResponse.EventId)
        /// {
        ///     Location = "Virtual Meeting"
        /// };
        /// await client.EditEvent(editParams);
        /// 
        /// // Get all events for a contact
        /// var events = await client.GetEventsAttachedToContact("contact-123");
        /// </summary>
    }
}
