using EngineeringToolbox.Domain.Settings;

namespace EngineeringToolbox.Api.Extensions
{
    public static class SettingsExtensions
    {
        public static void ResolveSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new Settings(
                identitySecret: configuration.GetSection("Identity")["IdentitySecret"],
                tokenExpiresInMiliSeconds: configuration.GetSection("Identity").GetValue<int>("TokenExpiresInMiliSeconds"),
                tokenAvailableDomains: configuration.GetSection("Identity")["TokenAvailableDomains"],
                tokenEmmiter: configuration.GetSection("Identity")["TokenEmmiter"],
                emailAdress: configuration.GetSection("Email")["HostAdress"],
                emailPassword: configuration.GetSection("Email")["HostCode"]
                );

            services.AddSingleton<ISettings>(settings);

        }
    }
}
