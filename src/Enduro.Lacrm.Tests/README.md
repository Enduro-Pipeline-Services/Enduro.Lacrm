# Enduro.Lacrm.Tests

Comprehensive test suite for the LACRM API wrapper library.

## Overview

This test project provides extensive coverage for the Enduro.Lacrm library, including:
- **Parameter validation tests** - Unit tests ensuring all parameter objects validate correctly
- **Integration tests** - Example tests demonstrating API usage with mocked HttpClient

## Test Structure

```
Tests/
├── Parameters/          # Parameter validation unit tests
│   ├── TaskParameterTests.cs
│   ├── EventParameterTests.cs
│   ├── NoteParameterTests.cs
│   ├── PipelineParameterTests.cs
│   ├── GroupParameterTests.cs
│   └── ContactParameterTests.cs
└── Integration/         # Integration tests with mocked HTTP responses
    ├── TaskApiTests.cs
    ├── EventApiTests.cs
    └── NoteApiTests.cs
```

## Running the Tests

### Using Visual Studio
1. Open `Enduro.Lacrm.sln` in Visual Studio
2. Open **Test Explorer** (Test > Test Explorer)
3. Click "Run All Tests" or right-click specific tests to run them individually

### Using .NET CLI

**Run all tests:**
```bash
dotnet test
```

**Run tests from the test project directory:**
```bash
cd src\Enduro.Lacrm.Tests
dotnet test
```

**Run tests with detailed output:**
```bash
dotnet test --verbosity detailed
```

**Run tests with code coverage:**
```bash
dotnet test --collect:"XPlat Code Coverage"
```

**Run specific test class:**
```bash
dotnet test --filter "FullyQualifiedName~TaskParameterTests"
```

**Run specific test method:**
```bash
dotnet test --filter "FullyQualifiedName~CreateTaskParams_ValidData_ReturnsSuccessfulValidation"
```

### Using Visual Studio Code
1. Install the "C# Dev Kit" extension
2. Open the Testing view (beaker icon in the sidebar)
3. Run individual tests or entire test suites

## Test Categories

### Parameter Validation Tests

These unit tests verify that parameter validation logic works correctly:

- **Success cases** - Valid parameters pass validation
- **Failure cases** - Invalid parameters fail with appropriate error messages
- **Edge cases** - Boundary conditions and special scenarios

Example:
```csharp
[Fact]
public void CreateTaskParams_ValidData_ReturnsSuccessfulValidation()
{
    var parameters = new CreateTaskParams(
        contactId: "12345",
        dueDate: "2024-12-31",
        name: "Test Task",
        description: "Test Description"
    );

    var result = parameters.Validate();

    Assert.True(result.Success);
}
```

### Integration Tests

These tests demonstrate real-world API usage patterns with mocked HTTP responses:

- **CRUD operations** - Create, Read, Update, Delete
- **List operations** - Fetching collections of resources
- **Complex scenarios** - Multi-step workflows

Example:
```csharp
[Fact]
public async Task CreateTask_ValidParameters_ReturnsSuccessResponse()
{
    // Arrange - Mock HTTP response
    var responseJson = @"{""Success"": true, ""TaskId"": ""task-12345""}";
    // ... setup mock ...

    // Act - Call the API
    var parameters = new CreateTaskParams(
        contactId: "contact-123",
        dueDate: "2024-12-31",
        name: "Complete Project",
        description: "Finish the integration tests"
    );
    var response = await _lacrmClient.CreateTask(parameters);

    // Assert - Verify response
    Assert.True(response.Success);
    Assert.Equal("task-12345", response.TaskId);
}
```

## Writing New Tests

### Adding Parameter Tests

1. Create or open the appropriate test file in `Tests/Parameters/`
2. Add a new test method with the `[Fact]` attribute
3. Follow the Arrange-Act-Assert pattern
4. Test both success and failure scenarios

### Adding Integration Tests

1. Create or open the appropriate test file in `Tests/Integration/`
2. Mock the `HttpMessageHandler` to return your desired response
3. Call the API method through `LacrmClient`
4. Assert on the response

Example template:
```csharp
[Fact]
public async Task MethodName_Scenario_ExpectedResult()
{
    // Arrange
    var responseJson = @"{""Success"": true}";
    _mockHttpMessageHandler.Protected()
        .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        )
        .ReturnsAsync(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseJson)
        });

    var parameters = new SomeParams("value");

    // Act
    var response = await _lacrmClient.SomeMethod(parameters);

    // Assert
    Assert.True(response.Success);
}
```

## Dependencies

- **xUnit** - Test framework
- **Moq** - Mocking library for creating test doubles
- **Microsoft.NET.Test.Sdk** - Test platform
- **coverlet.collector** - Code coverage collector

## Best Practices

1. **One Assert Per Test** - Each test should verify one specific behavior
2. **Clear Test Names** - Use `MethodName_Scenario_ExpectedResult` naming convention
3. **Arrange-Act-Assert** - Structure tests in three clear sections
4. **Mock External Dependencies** - Use Moq to mock HttpClient and other dependencies
5. **Test Edge Cases** - Include tests for null values, empty strings, invalid formats
6. **Document Complex Tests** - Add comments explaining the scenario being tested

## Continuous Integration

These tests are designed to run in CI/CD pipelines:

```yaml
# Example GitHub Actions workflow
- name: Run tests
  run: dotnet test --no-build --verbosity normal
```

## Code Coverage

To generate a code coverage report:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

Coverage reports will be generated in `TestResults/` directory.

## Troubleshooting

**Tests not appearing in Test Explorer:**
- Rebuild the solution
- Restart Visual Studio
- Check that test methods have the `[Fact]` attribute

**Tests failing unexpectedly:**
- Verify package versions match between main project and test project
- Check that mock setups match the expected HTTP calls
- Review parameter validation logic in the main library

**Build errors:**
- Run `dotnet restore` to restore NuGet packages
- Ensure .NET 10 SDK is installed
- Check project references are correct

## Contributing

When adding new features to the main library:
1. Add parameter validation tests for new parameter classes
2. Add integration tests showing usage examples
3. Ensure all tests pass before submitting PR
4. Maintain test coverage above 80%

## Support

For questions or issues:
- Check the main project README at `../../README.md`
- Review existing tests for examples
- Open an issue on the project repository
