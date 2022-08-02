namespace com.tweetapp.service
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;

    public static class TokenValidationHandler
    {
        public static void GetTokenValidationParameters(this JwtBearerOptions options, IConfiguration configuration)
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("TokenSecretKey"))),
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = configuration.GetSection("TokenSettings")["Issuer"],
                ValidAudience = configuration.GetSection("TokenSettings")["Audience"],
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        }
    }
}
