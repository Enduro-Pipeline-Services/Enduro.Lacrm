using System.Net;
using System.Net.Http;
using System.Threading;
using Moq;
using Moq.Protected;
using Xunit;

namespace Enduro.Lacrm.Tests.Tests.Integration
{
    /// <summary>
    /// Integration tests for Note API operations.
    /// These tests demonstrate how to call Note APIs with mocked HttpClient.
    /// </summary>
    public class NoteApiTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly LacrmClient _lacrmClient;

        public NoteApiTests()
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
        public async System.Threading.Tasks.Task CreateNote_ValidParameters_ReturnsSuccessResponse()
        {
            // Arrange
            var responseJson = @"{
                ""Success"": true,
                ""NoteId"": ""note-12345""
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
            var response = await _lacrmClient.CreateNote(
                contactId: "contact-123",
                note: "Called customer to follow up on their request. They were satisfied with the resolution."
            );

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal("note-12345", response.NoteId);
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
        /// // Create a new note for a contact
        /// var createResponse = await client.CreateNote(
        ///     contactId: "contact-123",
        ///     note: "Customer called to inquire about pricing. Sent them detailed quote via email."
        /// );
        /// 
        /// // Update the note with additional information
        /// var editParams = new EditNoteParams(createResponse.NoteId)
        /// {
        ///     Note = "Customer called to inquire about pricing. Sent them detailed quote via email. " +
        ///            "UPDATE: Customer approved quote and requested contract."
        /// };
        /// await client.EditNote(editParams);
        /// 
        /// // Get all notes for a contact (useful for viewing communication history)
        /// var notes = await client.GetNotesAttachedToContact("contact-123");
        /// 
        /// // Notes are great for:
        /// // - Recording phone call summaries
        /// // - Documenting email conversations
        /// // - Logging meeting outcomes
        /// // - Tracking customer interactions
        /// // - Maintaining a complete communication history
        /// </summary>
    }
}
