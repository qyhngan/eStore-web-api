using DataAccess.DTO.Member;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eStoreAPI.JWT
{
    public class JWTAuth : IJwtAuth
    {
        private readonly string _key;
        public JWTAuth(string key)
        {
            _key = key;
        }
        public string AuthenticateToken(string email, string role, string id = null)
        {
            // 1. create security token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 2, create private key to encrypted
            var tokenKey = Encoding.ASCII.GetBytes(_key);

            // 3. create token descriptor
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.Role, role)
                });
            if (id != null)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, id));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "eStore",
                Audience = "eStoreClient",
                Subject = claimsIdentity,
                Expires = System.DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                                       new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            // 4. create token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //5. return token
            return tokenHandler.WriteToken(token);

        }
    
    }
}
