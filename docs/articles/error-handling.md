# Error Handling

Proper error handling is essential for building robust applications with the Enduro.Lacrm library. This guide covers the exception types, error patterns, and best practices.

## Exception Hierarchy

The library provides three specific exception types:

```
Exception
└── ValidationException    - Parameter validation failures
└── HttpException         - HTTP communication errors
└── ApiException          - LACRM API errors
```

All exceptions inherit from `System.Exception` and include detailed error information.

## Exception Types

### ValidationException

Thrown when request parameters fail validation before the API call is made.

**When it occurs:**
- Required parameters are missing
- Parameter values are invalid
- Business rule validation fails

**Properties:**
- `Message` - Error description
- `ValidationResponse` - Detailed validation results
  - `Success` - Always false for exceptions
  - `Errors` - List of validation errors
    - `ParameterName` - Name of invalid parameter
    - `Message` - Specific error message

**Example:**

```csharp
using Enduro.Lacrm.Exceptions;

try
{
    var parameters = new EditTaskParams(null);  // Missing required TaskId
    await lacrm.EditTask(parameters);
}
catch (ValidationException ex)
{
    Console.WriteLine($"Validation failed: {ex.Message}");
    
    foreach (var error in ex.ValidationResponse.Errors)
    {
        Console.WriteLine($"  Parameter: {error.ParameterName}");
        Console.WriteLine($"  Error: {error.Message}");
    }
}
```

### HttpException

Thrown when HTTP communication with the LACRM API fails.

**When it occurs:**
- Network connectivity issues
- Timeout errors
- HTTP status code errors (non-200 responses)
- DNS resolution failures

**Example:**

```csharp
try
{
    var response = await lacrm.GetContact("contact-id");
}
catch (HttpException ex)
{
    Console.WriteLine($"HTTP communication failed: {ex.Message}");
    
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Underlying error: {ex.InnerException.Message}");
    }
}
```

### ApiException

Thrown when the LACRM API returns an error response.

**When it occurs:**
- Invalid API credentials
- Resource not found
- Permission denied
- API rate limiting
- Invalid data sent to API

**Example:**

```csharp
try
{
    var response = await lacrm.GetContact("invalid-id");
}
catch (ApiException ex)
{
    Console.WriteLine($"API error: {ex.Message}");
}
```

## Comprehensive Error Handling Pattern

### Basic Pattern

```csharp
using Enduro.Lacrm.Exceptions;

try
{
    var response = await lacrm.SearchContacts("test");
    // Process successful response
}
catch (ValidationException ex)
{
    // Handle validation errors
    Console.WriteLine("Invalid parameters:");
    foreach (var error in ex.ValidationResponse.Errors)
    {
        Console.WriteLine($"  {error.ParameterName}: {error.Message}");
    }
}
catch (ApiException ex)
{
    // Handle API errors
    Console.WriteLine($"API error: {ex.Message}");
}
catch (HttpException ex)
{
    // Handle communication errors
    Console.WriteLine($"Communication error: {ex.Message}");
}
catch (Exception ex)
{
    // Handle unexpected errors
    Console.WriteLine($"Unexpected error: {ex.Message}");
}
```

### Retry Pattern for Transient Failures

```csharp
public async Task<SearchContactsResponse> SearchWithRetry(
    string searchTerm, 
    int maxRetries = 3)
{
    for (int attempt = 1; attempt <= maxRetries; attempt++)
    {
        try
        {
            return await _lacrm.SearchContacts(searchTerm);
        }
        catch (HttpException ex) when (attempt < maxRetries)
        {
            Console.WriteLine($"Attempt {attempt} failed: {ex.Message}");
            Console.WriteLine($"Retrying in {attempt * 2} seconds...");
            
            await Task.Delay(TimeSpan.FromSeconds(attempt * 2));
        }
        catch (HttpException ex)
        {
            // Final attempt failed
            Console.WriteLine($"All {maxRetries} attempts failed");
            throw;
        }
    }
    
    throw new InvalidOperationException("Should not reach here");
}
```

### Exponential Backoff

```csharp
public async Task<T> ExecuteWithBackoff<T>(
    Func<Task<T>> operation,
    int maxAttempts = 5)
{
    var delay = TimeSpan.FromSeconds(1);
    
    for (int attempt = 1; attempt <= maxAttempts; attempt++)
    {
        try
        {
            return await operation();
        }
        catch (HttpException) when (attempt < maxAttempts)
        {
            Console.WriteLine($"Attempt {attempt} failed, waiting {delay.TotalSeconds}s");
            await Task.Delay(delay);
            delay = TimeSpan.FromSeconds(Math.Min(delay.TotalSeconds * 2, 60));
        }
    }
    
    throw new InvalidOperationException($"Operation failed after {maxAttempts} attempts");
}

// Usage
var response = await ExecuteWithBackoff(() => 
    lacrm.SearchContacts("test")
);
```

## Handling Specific Scenarios

### Resource Not Found

```csharp
public async Task<Contact?> TryGetContact(string contactId)
{
    try
    {
        var response = await _lacrm.GetContact(contactId);
        return response.Contact;
    }
    catch (ApiException ex) when (ex.Message.Contains("not found"))
    {
        Console.WriteLine($"Contact {contactId} not found");
        return null;
    }
}
```

### Authentication Errors

```csharp
try
{
    await lacrm.GetUserInfo();
}
catch (ApiException ex) when (
    ex.Message.Contains("authentication") || 
    ex.Message.Contains("credentials"))
{
    Console.WriteLine("Authentication failed. Please check API Token and User Code.");
    // Re-prompt for credentials or exit
}
```

### Rate Limiting

```csharp
public async Task<T> ExecuteWithRateLimit<T>(Func<Task<T>> operation)
{
    try
    {
        return await operation();
    }
    catch (ApiException ex) when (ex.Message.Contains("rate limit"))
    {
        Console.WriteLine("Rate limit reached, waiting 60 seconds...");
        await Task.Delay(TimeSpan.FromSeconds(60));
        
        // Retry once
        return await operation();
    }
}
```

## Validation Error Handling

### Pre-validation

Validate parameters before API calls to fail fast:

```csharp
public async Task<CreateTaskResponse> CreateTaskSafe(
    string contactId,
    string dueDate,
    string name,
    string description)
{
    // Pre-validate
    var errors = new List<string>();
    
    if (string.IsNullOrWhiteSpace(contactId))
        errors.Add("Contact ID is required");
    
    if (string.IsNullOrWhiteSpace(name))
        errors.Add("Task name is required");
    
    if (!DateTime.TryParseExact(dueDate, "yyyy-MM-dd", null, 
        DateTimeStyles.None, out _))
        errors.Add("Due date must be in YYYY-MM-DD format");
    
    if (errors.Any())
    {
        throw new ArgumentException($"Validation failed:\n{string.Join("\n", errors)}");
    }
    
    // Now call API
    return await _lacrm.CreateTask(contactId, dueDate, name, description);
}
```

### Collecting Multiple Validation Errors

```csharp
public async Task<List<string>> CreateMultipleContacts(
    List<CreateContactParams> contacts)
{
    var createdIds = new List<string>();
    var errors = new List<string>();

    foreach (var contact in contacts)
    {
        try
        {
            var response = await _lacrm.CreateContact(contact);
            createdIds.Add(response.ContactId);
        }
        catch (ValidationException ex)
        {
            var errorDetails = string.Join(", ", 
                ex.ValidationResponse.Errors.Select(e => 
                    $"{e.ParameterName}: {e.Message}"));
            
            errors.Add($"Failed to create {contact.FirstName} {contact.LastName}: {errorDetails}");
        }
    }

    if (errors.Any())
    {
        Console.WriteLine($"Created {createdIds.Count} contacts with {errors.Count} errors:");
        foreach (var error in errors)
        {
            Console.WriteLine($"  - {error}");
        }
    }

    return createdIds;
}
```

## Logging Best Practices

### Structured Logging

```csharp
using Microsoft.Extensions.Logging;

public class LacrmService
{
    private readonly LacrmClient _lacrm;
    private readonly ILogger<LacrmService> _logger;

    public LacrmService(LacrmClient lacrm, ILogger<LacrmService> logger)
    {
        _lacrm = lacrm;
        _logger = logger;
    }

    public async Task<Contact?> GetContactSafe(string contactId)
    {
        try
        {
            _logger.LogInformation("Fetching contact {ContactId}", contactId);
            var response = await _lacrm.GetContact(contactId);
            _logger.LogInformation("Successfully retrieved contact {ContactId}", contactId);
            return response.Contact;
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, 
                "Validation failed for contact {ContactId}: {Errors}", 
                contactId,
                string.Join(", ", ex.ValidationResponse.Errors.Select(e => e.Message)));
            return null;
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, 
                "API error retrieving contact {ContactId}", 
                contactId);
            return null;
        }
        catch (HttpException ex)
        {
            _logger.LogError(ex, 
                "HTTP error retrieving contact {ContactId}", 
                contactId);
            throw;  // Rethrow for transient errors
        }
    }
}
```

## Error Response Helper

Create a helper class to standardize error responses:

```csharp
public class OperationResult<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorType { get; set; }
    public List<string> ValidationErrors { get; set; } = new();

    public static OperationResult<T> Ok(T data) => new()
    {
        Success = true,
        Data = data
    };

    public static OperationResult<T> Fail(string message, string type = "Error") => new()
    {
        Success = false,
        ErrorMessage = message,
        ErrorType = type
    };

    public static OperationResult<T> ValidationFail(IEnumerable<string> errors) => new()
    {
        Success = false,
        ErrorType = "Validation",
        ValidationErrors = errors.ToList()
    };
}

// Usage
public async Task<OperationResult<Contact>> GetContactResult(string contactId)
{
    try
    {
        var response = await _lacrm.GetContact(contactId);
        return OperationResult<Contact>.Ok(response.Contact);
    }
    catch (ValidationException ex)
    {
        var errors = ex.ValidationResponse.Errors
            .Select(e => $"{e.ParameterName}: {e.Message}");
        return OperationResult<Contact>.ValidationFail(errors);
    }
    catch (ApiException ex)
    {
        return OperationResult<Contact>.Fail(ex.Message, "API");
    }
    catch (HttpException ex)
    {
        return OperationResult<Contact>.Fail(ex.Message, "HTTP");
    }
}
```

## Circuit Breaker Pattern

For production systems, implement a circuit breaker to prevent cascading failures:

```csharp
public class CircuitBreaker
{
    private int _failureCount = 0;
    private DateTime? _lastFailure;
    private readonly int _threshold = 5;
    private readonly TimeSpan _timeout = TimeSpan.FromMinutes(5);

    public async Task<T> Execute<T>(Func<Task<T>> operation)
    {
        if (_failureCount >= _threshold && 
            _lastFailure.HasValue && 
            DateTime.UtcNow - _lastFailure.Value < _timeout)
        {
            throw new InvalidOperationException(
                "Circuit breaker is open. Service temporarily unavailable.");
        }

        try
        {
            var result = await operation();
            _failureCount = 0;  // Reset on success
            return result;
        }
        catch (HttpException)
        {
            _failureCount++;
            _lastFailure = DateTime.UtcNow;
            throw;
        }
    }
}
```

## Testing Error Handling

```csharp
[Test]
public async Task CreateContact_WithInvalidEmail_ThrowsValidationException()
{
    var parameters = new CreateContactParams
    {
        FirstName = "Test",
        Email = new[] { new EmailParams { Text = "" } }  // Invalid
    };

    await Assert.ThrowsAsync<ValidationException>(() => 
        _lacrm.CreateContact(parameters)
    );
}

[Test]
public async Task GetContact_WithInvalidId_ThrowsApiException()
{
    await Assert.ThrowsAsync<ApiException>(() => 
        _lacrm.GetContact("invalid-id")
    );
}
```

## Best Practices

1. **Catch specific exceptions first**: Order catch blocks from most specific to least specific
2. **Don't swallow exceptions**: Log or handle appropriately, don't hide errors
3. **Use retry logic for transient failures**: HTTP errors may succeed on retry
4. **Validate early**: Check parameters before making API calls
5. **Log with context**: Include relevant IDs and parameters in logs
6. **Return user-friendly messages**: Don't expose technical details to end users
7. **Use cancellation tokens**: Allow operations to be cancelled
8. **Implement timeouts**: Prevent indefinite waiting
9. **Monitor error rates**: Track errors to identify systemic issues
10. **Document error scenarios**: Help consumers understand possible failures

## See Also

- [Getting Started](getting-started.md)
- [Pagination](pagination.md)
- [Migration Guide](migration-v1-to-v2.md)
