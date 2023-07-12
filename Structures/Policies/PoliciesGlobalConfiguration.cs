namespace WebApiPolly.Structures.Policies
{
    /// <summary>
    /// It Defines the Global Configuration for all Policies
    /// </summary>
    public class PoliciesGlobalConfiguration
    {
        /// <summary>
        /// Maximum Retries
        /// </summary>
        public int MaxRetries { get; set; }

        /// <summary>
        /// Time to wait for the next operation (Retry or Timeout)
        /// </summary>
        public int TimeToWaitInMilliseconds { get; set; }
    }
}