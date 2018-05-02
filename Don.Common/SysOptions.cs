namespace Don.Common
{
    public class SysOptions
    {
        public string DbProvider { get; set; }
        public string ConnectionString { get; set; }
    }

    public class JwtOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int Expiry { get; set; }

        public string Secret { get; set; }
    }
}
