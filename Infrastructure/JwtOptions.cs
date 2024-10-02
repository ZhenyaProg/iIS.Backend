namespace Infrastructure
{
    public class JwtOptions
    {
        public string ISSUER { get; set; } = string.Empty;
        public string AUDIENCE { get; set; } = string.Empty;
        public string KEY { get; set; } = string.Empty;
        public int Expires { get; set; }
    }
}