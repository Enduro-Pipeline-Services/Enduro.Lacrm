using Enduro.Lacrm.Parameters;
using Xunit;

namespace Enduro.Lacrm.Tests.Tests.Parameters
{
    public class ContactParameterTests
    {
        [Fact]
        public void CreateContactParams_WithFullName_ReturnsSuccessfulValidation()
        {
            // Arrange
            var parameters = new CreateContactParams
            {
                FullName = "John Doe",
                CompanyId = "company-123"
            };

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void CreateContactParams_WithSeparateNameFields_ReturnsSuccessfulValidation()
        {
            // Arrange
            var parameters = new CreateContactParams
            {
                FirstName = "John",
                LastName = "Doe",
                CompanyId = "company-123"
            };

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void CreateContactParams_WithBothFullNameAndSeparateFields_FailsValidation()
        {
            // Arrange - Using both naming methods should fail
            var parameters = new CreateContactParams
            {
                FullName = "John Doe",
                FirstName = "John",
                LastName = "Doe",
                CompanyId = "company-123"
            };

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, e => e.Parameter == "FullName");
        }

        [Fact]
        public void CreateContactParams_WithCompanyNameAndId_FailsValidation()
        {
            // Arrange - Using both company selection methods should fail
            var parameters = new CreateContactParams
            {
                FullName = "John Doe",
                CompanyName = "Acme Corp",
                CompanyId = "company-123"
            };

            // Act
            var result = parameters.Validate();

            // Assert
            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, e => e.Parameter == "CompanyName" || e.Parameter == "CompanyId");
        }
    }
}
