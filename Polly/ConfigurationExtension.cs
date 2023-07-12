using WebApiPolly.Structures.Policies;

namespace WebApiPolly.Polly
{
    public static class ConfigurationExtension
    {
        public static PoliciesSettings? GetPoliciesSettings(this IConfiguration config)
        {
            return config.GetSection(nameof(PoliciesSettings)).Get<PoliciesSettings>();
        }
    }
}
