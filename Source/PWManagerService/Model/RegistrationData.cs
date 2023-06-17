using System.Runtime.InteropServices;

namespace PWManagerService.Model
{
    /// <summary>
    /// Klasse zum Uebergeben der Benutzerdaten bei Accounterstellung
    /// </summary>
    public class RegistrationData
    {
        public RegistrationData(string email, string hashedPassword, string username, string salt, DateTime agbAcceptedAt, string passwordHint)
        {
            this.Email = email;
            this.HashedPassword = hashedPassword;
            this.Username = username;
            this.Salt = salt;
            this.AgbAcceptedAt = agbAcceptedAt;
            this.PasswordHint = passwordHint;
        }

        public string Email { get; private set; }
        public string HashedPassword { get; private set; }
        public string Username { get; private set; }
        public string Salt { get; private set; }
        public DateTime AgbAcceptedAt { get; private set; }
        public string PasswordHint { get; private set; }
    }
}
