namespace WebApiPolly.Polly
{
    public static class PollyPolicyRegistryExtension
    {
        public static IServiceCollection AddPollyPolicyRegistry(this IServiceCollection services, IConfiguration config)
        {
            var policiesSettings = config.GetPoliciesSettings();

            var registry = PollyPolicyRegistry.Create(policiesSettings);

            services.AddPolicyRegistry(registry);

            return services;
        }
    }
}
