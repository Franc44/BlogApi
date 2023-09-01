using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BlogApi.Security
{
    internal class TokenGenVal
    {
        public TokenGenVal()
        {

        }
        static public string CrearToken(string Nombre, string Rol, string secretKey, string audienceToken, string issuerToken, DateTime expires, out DateTime ExpireDate)
        {    
            //Ciframos y se firma
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, Nombre),
                new Claim(ClaimTypes.Role, Rol)
            });

            ExpireDate = expires;

            // create token to the user 
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                subject: claimsIdentity,
                notBefore: DateTime.Now,
                expires: expires,
                signingCredentials: signingCredentials);

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtTokenString;
        }

        static public bool ValidacionTokenManual(string Token, string secretKey, string audienceToken, string issuerToken)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidAudience = audienceToken,
                ValidIssuer = issuerToken,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey))
            };

            SecurityToken validateToken;
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            if(handler.CanReadToken(Token))
            {
                var user = handler.ValidateToken(Token, validationParameters, out validateToken);

                string roles = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
                if(roles != "")
                {
                    return true;
                }
            }

            return false;
        }
    }
}