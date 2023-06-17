namespace PWManagerService.Model
{
    /// <summary>
    /// Klasse zum Uebergeben der Daten zur Authentifizierung / Token Erstellung
    /// </summary>
    public class AuthentificationData
    {
        public AuthentificationData(string hashedPassword, string email)
        {
            this.HashedPassword = hashedPassword;
            this.Email = email;
        }

        public string HashedPassword { get; private set; }
        public string Email { get; private set; }
    }
}
