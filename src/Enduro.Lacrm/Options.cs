using JetBrains.Annotations;

namespace Enduro.Lacrm
{
    [PublicAPI]
    public class Options
    {
        public string ApiUrl { get; set; } = null!;
        public string UserCode { get; set; } = null!;
        public string ApiToken { get; set; } = null!;
    }
}