using System;
using System.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace DataAccess
{
    public class Token
    {
        public string jwtToken { get; set; }
        public DateTime expires { get; set; }
    }

    internal static class TokenGenerator
    {
        public static Token GenerateTokenJwt(string username)
        {
            Token token = new Token();

            // appsetting for Token JWT
            var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
            var expireTime = ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // create a claimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) });

            DateTime now = DateTime.UtcNow;
            DateTime expires = now.AddMinutes(Convert.ToInt32(expireTime));

            // create token to the user
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                subject: claimsIdentity,
                notBefore: now,
                expires: expires,
                signingCredentials: signingCredentials);

            token.jwtToken = tokenHandler.WriteToken(jwtSecurityToken);
            token.expires = expires;

            return token;
        }
    }
}
