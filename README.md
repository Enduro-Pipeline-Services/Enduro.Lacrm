# Less Annoying CRM DotNet API Wrapper

The following is a simple wrapper around the official [Less Annoying CRM API](https://lessannoyingcrm.com/help/topic/API).

## Usage

To use, simply pass in your API Token, User Code, and the Less Annoying CRM Api 
Endpoint as part of the options, like so:

```csharp
static async Task Main()
{
    var client = new HttpClient();
    var options = new Options
    {
        ApiToken = "API_TOKEN_HERE",
        UserCode = "USER_CODE_HERE",
        ApiUrl = "https://api.lessannoyingcrm.com"
    };
    var lacrm = new LacrmClient(client, options);
    
    var response = await lacrm.SearchContacts("Search Terms");
    Console.WriteLine($"Found: {response.Result.Count()} matching contacts");
}
```