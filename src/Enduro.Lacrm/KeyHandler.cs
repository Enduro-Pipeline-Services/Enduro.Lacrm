using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Enduro.Lacrm.Extensions;

namespace Enduro.Lacrm
{
    public class KeyHandler : DelegatingHandler
    {
        private readonly string _escapedApiToken;
        private readonly string _escapedUserCode;

        public KeyHandler(string apiToken, string userCode)
        {
            _escapedApiToken = Uri.EscapeDataString(apiToken);
            _escapedUserCode = Uri.EscapeDataString(userCode);
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            request.RequestUri = request.RequestUri
                .AddParameter("APIToken", _escapedApiToken)
                .AddParameter("UserCode", _escapedUserCode);

            return base.SendAsync(request, cancellationToken);
        }
    }
}