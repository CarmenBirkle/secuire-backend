using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PWManagerService
{
    public class TokenService
    {
        private const int ExpirationMinutes = 30; // ToDo: In Konfig auslagern

        public static string GetUserMail(string jwtToken)
        {
            string email = string.Empty;

            try
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = handler.ReadJwtToken(jwtToken);

                string emailTypeString = @"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
                email = token.Claims.Where(c => c.Type == emailTypeString).Single().Value;
            }
            catch(Exception ex)
            {
                //ToDo: Loggen
                return string.Empty;
            }

            return email;
        }

        public string CreateToken(IdentityUser user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
            var token = CreateJwtToken(
                CreateClaims(user),
                CreateSigningCredentials(),
                expiration
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
            DateTime expiration) =>
            new(
                "apiWithAuthBackend",
                "apiWithAuthBackend",
                claims,
                expires: expiration,
                signingCredentials: credentials
            );

        private List<Claim> CreateClaims(IdentityUser user)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "TokenForTheApiWithAuth"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                };
                return claims;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("!SomethingSecret!") // ToDo: Auslagern
                ),
                SecurityAlgorithms.HmacSha256
            );
        }
    }
}
