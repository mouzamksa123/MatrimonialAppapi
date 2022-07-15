using System;

namespace CAL.matrimony
{
    public class User
    {
        public User()
        {
            UserName = "test";
            DisplayName = "Test Display Name";
            EmailId = "test@test.com";
        }
       
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public Guid Id { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
