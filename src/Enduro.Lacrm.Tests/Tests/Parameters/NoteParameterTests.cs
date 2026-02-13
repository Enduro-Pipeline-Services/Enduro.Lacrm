using Enduro.Lacrm.Parameters;
using Xunit;

namespace Enduro.Lacrm.Tests.Tests.Parameters
{
    public class NoteParameterTests
    {
        [Fact]
        public void CreateNoteParams_ValidContactId_ReturnsSuccessfulValidation()
        {
            // Arrange
            var parameters = new CreateNoteParams("contact-123");

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void CreateNoteParams_WithNoteText_ReturnsSuccessfulValidation()
        {
            // Arrange
            var parameters = new CreateNoteParams("contact-123", "This is a test note");

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.True(result.Success);
            Assert.Equal("This is a test note", parameters.Note);
        }

        [Fact]
        public void CreateNoteParams_NullContactId_FailsValidation()
        {
            // Arrange
            var parameters = new CreateNoteParams(null!);

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, e => e.Parameter == "ContactId");
        }

        [Fact]
        public void EditNoteParams_ValidNoteId_ReturnsSuccessfulValidation()
        {
            // Arrange
            var parameters = new EditNoteParams("note-123")
            {
                Note = "Updated note content"
            };

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Updated note content", parameters.Note);
        }
    }
}
