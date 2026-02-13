using Enduro.Lacrm.Parameters;
using Xunit;

namespace Enduro.Lacrm.Tests.Tests.Parameters
{
    public class GroupParameterTests
    {
        [Fact]
        public void CreateGroupParams_ValidGroupName_ReturnsSuccessfulValidation()
        {
            // Arrange
            var parameters = new CreateGroupParams("TestGroup");

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void CreateGroupParams_SpacesInName_ReplacedWithUnderscores()
        {
            // Arrange
            var parameters = new CreateGroupParams("Test Group Name");

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Test_Group_Name", parameters.GroupName);
        }

        [Fact]
        public void CreateGroupParams_EmptyGroupName_FailsValidation()
        {
            // Arrange
            var parameters = new CreateGroupParams("");

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, e => e.Parameter == "GroupName");
        }

        [Fact]
        public void AddContactToGroupParams_ValidData_ReturnsSuccessfulValidation()
        {
            // Arrange
            var parameters = new AddContactToGroupParams("contact-123", "TestGroup");

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public void AddContactToGroupParams_NullContactId_FailsValidation()
        {
            // Arrange
            var parameters = new AddContactToGroupParams(null!, "TestGroup");

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, e => e.Parameter == "ContactId");
        }
    }
}
