using static WebApiPolly.Structures.DefinedTypes;

namespace WebApiPolly.Structures.Policies
{
    /// <summary>
    /// It defines the Policy Info
    /// </summary>
    public class PolicyInfo
    {
        /// <summary>
        /// Maximum Retries
        /// This overwrites the value defined in the global configuration if not null
        /// </summary>
        public int? MaxRetries { get; set; }

        /// <summary>
        /// Name or identifier for the Policy
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Time to wait for the next operation.
        /// This overwrites the value defined in the global configuration if not null
        /// </summary>
        public int? TimeToWaitInMilliseconds { get; set; }

        /// <summary>
        /// Policy Types allowed (Retry, WaitAndRetry, Timeout)
        /// </summary>
        public PolicyType Type { get; set; }
    }
}