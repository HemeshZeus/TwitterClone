namespace com.tweetapp.service
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    public class GenerateTokenHandler
    {
        public IConfiguration Configuration { get; }
        public GenerateTokenHandler(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public string GenerateToken(string userId, string name, string Role = "Default")
        {
            //Create a List of Claims, Keep claims name short    
            var tokenClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToLower()),
                new Claim("Role", Role),
                new Claim("userid", userId.ToLower()),
                new Claim("name", name)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("TokenSecretKey")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                            issuer: Configuration.GetSection("TokenSettings")["Issuer"],
                            audience: Configuration.GetSection("TokenSettings")["Audience"],
                            tokenClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
