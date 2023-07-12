using Polly;
using Polly.Registry;
using RestSharp;
using Serilog;

namespace WebApiPolly.Infrastructure
{
    public class Connector
    {
        private readonly IPolicyRegistry<string> _policyRegistry;

        public Connector(IPolicyRegistry<string> policyRegistry)
        {
            _policyRegistry = policyRegistry;
        }

        public async Task<string?> SendRequest()
        {
            var request = new RestRequest("http://localhost:62737/WeatherForecast");
            var policyResult = await TrySendRequestAsync(request, "FallbackPolicy");
            
            if (policyResult.FinalHandledResult != null)
            {
                Log.Error($"Exception: {policyResult.FinalHandledResult.ErrorMessage}");
                throw new Exception(policyResult.FinalHandledResult.ErrorMessage);
            }

            if (policyResult.Result is { ErrorException: { } })
            {
                Log.Error($"Exception: {policyResult.Result.ErrorException.Message}");
                throw new Exception(policyResult.Result.ErrorException.Message);
            }

            var response = policyResult.Result.Content;
            Log.Information($"Response: {response}");

            return response;
        }

        private async Task<PolicyResult<RestResponse>> TrySendRequestAsync(RestRequest request, string policyName)
        {
            Log.Information("Calling External Endpoint");

            var policy = _policyRegistry
                .Where(pm => policyName.Equals(pm.Key))
                .Select(p => (AsyncPolicy<RestResponse>)p.Value).ToArray()[0];
            
            return await policy.ExecuteAndCaptureAsync( async (token) =>
            {
                Log.Information($"Calling External Endpoint by Policy: {policyName}");
                return await new RestClient().ExecuteAsync(request, token);
            }, CancellationToken.None);
        }
    }
}
