using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;




namespace quizapi.Infrastructure
{


    public class JWT
    {
        public static string GenerateToken(Dictionary<string, string> claimsToBeAdded, string key)
        {
            var TokenHandler = new JwtSecurityTokenHandler();
            var TokenKey = Encoding.ASCII.GetBytes(key);
            var expiresAt = DateTime.Now.AddDays(30);

            var claimsIdentity = new ClaimsIdentity();
            foreach (var item in claimsToBeAdded)
            {
                claimsIdentity.AddClaim(new Claim(item.Key, item.Value));
            }

            var TokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claimsIdentity,
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(TokenKey), SecurityAlgorithms.HmacSha256Signature),
            };

            var Token = TokenHandler.CreateToken(TokenDescriptor);
            return TokenHandler.WriteToken(Token);
        }
    }
}
