namespace EngineeringToolbox.Domain.Settings
{
    public interface ISettings
    {
        string IdentitySecret { get; }
        int TokenExpiresInMiliSeconds { get; }
        string TokenAvailableDomains { get; }
        string TokenEmmiter { get; }
        string EmailAdress { get; }
        string EmailPassword { get; }
    }


    public class Settings : ISettings
    {
        public string IdentitySecret { get; }
        public int TokenExpiresInMiliSeconds { get; }
        public string TokenAvailableDomains { get; }
        public string TokenEmmiter { get; }
        public string EmailAdress { get; }
        public string EmailPassword { get; }

        public Settings(string identitySecret,
                        int tokenExpiresInMiliSeconds,
                        string tokenAvailableDomains,
                        string tokenEmmiter,
                        string emailAdress,
                        string emailPassword)
        {
            IdentitySecret = identitySecret;
            TokenExpiresInMiliSeconds = tokenExpiresInMiliSeconds;
            TokenAvailableDomains = tokenAvailableDomains;
            TokenEmmiter = tokenEmmiter;
            EmailAdress = emailAdress;
            EmailPassword = emailPassword;
        }
    }
}
