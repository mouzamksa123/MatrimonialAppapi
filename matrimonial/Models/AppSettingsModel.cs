namespace matrimonial.Models
{

    public class AppSettingsModel
    {
        public LDAPDetailsObject LDAPDetails { get; set; }
        public JWTObject JWT { get; set; }
    }
    public class LDAPDetailsObject
    {
        public string Domain { get; set; }
        public string DefaultOU { get; set; }
        public string DefaultRootOU { get; set; }
        public string ServiceUser { get; set; }
        public string ServicePassword { get; set; }
    }
    public class JWTObject
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
        public string TokenValidityInMinutes { get; set; }
        public string RefreshTokenValidityInDays { get; set; }
    }
}
