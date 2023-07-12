using System.Net;
using Polly.Registry;
using Polly.Timeout;
using Polly;
using RestSharp;
using Serilog;
using WebApiPolly.Structures;
using WebApiPolly.Structures.Policies;

namespace WebApiPolly.Polly
{
    public static class PollyPolicyRegistry
    {
        public static PolicyRegistry Create(PoliciesSettings? policiesSettings)
        {
            var registry = new PolicyRegistry();

            if (policiesSettings is null) return registry;
            
            var maxRetries = policiesSettings.PoliciesGlobalConfiguration.MaxRetries;

            foreach (var policyInfo in policiesSettings.Policies)
            {
                var policyName = policyInfo.Name;
                var timeToWaitInMilliseconds = policyInfo.TimeToWaitInMilliseconds ?? policiesSettings.PoliciesGlobalConfiguration.TimeToWaitInMilliseconds;

                if (policyInfo.MaxRetries.HasValue) maxRetries = policyInfo.MaxRetries.Value;

                IAsyncPolicy<RestResponse> policy = policyInfo.Type switch
                {
                    DefinedTypes.PolicyType.Retry => Policy.HandleResult<RestResponse>(r => !r.IsSuccessful).RetryAsync(maxRetries),
                    DefinedTypes.PolicyType.WaitAndRetry => Policy.HandleResult<RestResponse>(r => !r.IsSuccessful).WaitAndRetryAsync(maxRetries, i => TimeSpan.FromMilliseconds(timeToWaitInMilliseconds)),
                    DefinedTypes.PolicyType.Timeout => Policy.TimeoutAsync<RestResponse>(TimeSpan.FromMilliseconds(timeToWaitInMilliseconds), TimeoutStrategy.Optimistic),
                    DefinedTypes.PolicyType.Fallback => Policy.HandleResult<RestResponse>(r => !r.IsSuccessful).FallbackAsync(FallbackAction, OnFallbackAsync),
                    _ => throw new ArgumentOutOfRangeException()
                };

                registry.Add(policyName, policy);
            }

            return registry;
        }

        private static Task OnFallbackAsync(DelegateResult<RestResponse> response, Context context)
        {
            Log.Information($"OnFallbackAsync executing. CorrelationID: {context.CorrelationId}");
            return Task.CompletedTask;
        }

        private static Task<RestResponse> FallbackAction(DelegateResult<RestResponse> responseToFailedRequest, Context context, CancellationToken cancellationToken)
        {
            Log.Information("Fallback action is executing");
            var errorMessage = $"The fallback executed, the original error was {responseToFailedRequest.Result.ErrorMessage}";
            Log.Error(errorMessage);

            RestResponse response = new RestResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Content = "Exception handled by Fallback policy",
                ResponseStatus = ResponseStatus.Completed,
                IsSuccessStatusCode = true
            };

            return Task.FromResult(response);
        }

    }
}
