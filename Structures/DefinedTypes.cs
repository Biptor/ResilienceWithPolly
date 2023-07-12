using System.Diagnostics.CodeAnalysis;

namespace WebApiPolly.Structures
{
    [ExcludeFromCodeCoverage]
    public static class DefinedTypes
    {
        public enum HttpMethodType
        {
            GET = 0,
            POST = 1,
            PUT = 2,
            DELETE = 3
        }
        public enum PolicyType
        {
            Retry,
            WaitAndRetry,
            Timeout,
            Fallback
        }
    }
}