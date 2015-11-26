using BusinessObjects.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Handlers
{
    public class AllowedOriginHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {           
            IEnumerable<string> values;
            //safely get origin value

            if (request.Headers.TryGetValues("Origin", out values))
            {
                var requestOrigin = values.First();
                if (IsOriginAllowed(requestOrigin))
                {
                    return GenerateSuccessResponse(request, cancellationToken, requestOrigin);
                }
            }
            else
            {
                var requestOrigin = request.Headers.Referrer.AbsoluteUri.Trim('/');
                if (IsOriginAllowed(requestOrigin))
                {
                    return GenerateSuccessResponse(request, cancellationToken, requestOrigin);
                }
            }

            return GenerateUnauthorizedResponse(request, cancellationToken);
        }

        private bool IsOriginAllowed(string requestOrigin)
        {
            //compare with allowed origins from config
            var allowedOrigins = AllowedOriginConfig.GetDomains();

            return requestOrigin != null && !string.IsNullOrEmpty(requestOrigin) && allowedOrigins.Contains(requestOrigin);
        }

        private Task<HttpResponseMessage> GenerateSuccessResponse(HttpRequestMessage request, CancellationToken cancellationToken, string requestOrigin)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith(
                    task =>
                    {
                        var response = task.Result;
                        response.Headers.Add("Access-Control-Allow-Origin", requestOrigin);
                        return response;
                    });
        }

        private Task<HttpResponseMessage> GenerateUnauthorizedResponse(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var unauthorizedResponse = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized origin.");
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(unauthorizedResponse);
            return tsc.Task;
        }
    }
}