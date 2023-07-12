namespace WebApiPolly.Structures.Policies
{
    /// <summary>
    /// It Define the policies for all Routes and Markets
    /// </summary>
    public class PoliciesSettings
    {
        /// <summary>
        /// Global Configuration, Applies to all Routes and Markets
        /// </summary>
        public PoliciesGlobalConfiguration PoliciesGlobalConfiguration { get; set; }

        /// <summary>
        /// List of policies to apply
        /// </summary>
        public List<PolicyInfo> Policies { get; set; }
    }
}