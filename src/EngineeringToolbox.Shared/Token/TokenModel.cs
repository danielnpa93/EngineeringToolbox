namespace EngineeringToolbox.Shared.Token
{
    public class TokenModel
    {
        public string Token { get; set; }
        public DateTime ExpiresIn { get; set; }

        public bool EmailConfirmed { get; set; }
    }
}
