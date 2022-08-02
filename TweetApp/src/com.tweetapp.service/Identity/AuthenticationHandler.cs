namespace com.tweetapp.service
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Net.Http.Headers;
    using Newtonsoft.Json.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Threading.Tasks;

    /// <summary>
    /// AuthenticationHandler static class to implement custom authentication for application - JwtToken.
    /// </summary>
    public static class AuthenticationHandler
    {
        public static void CustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication()
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = ctx =>
                    {
                        var appIdentity = (ClaimsIdentity)ctx.Principal.Identity;
                        var claimInToken = appIdentity.FindFirst("Role");
                        if (claimInToken != null)
                        {
                            appIdentity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, "D:" + claimInToken.Value));
                        }
                        return Task.CompletedTask;
                    },

                    OnForbidden = context =>
                    {
                        var payload = new JObject
                        (
                            new JProperty("error",
                                new JObject(
                                    new JProperty("description", "You are Unauthorized to access the resource."),
                                    new JProperty("responseCode", 403),
                                    new JProperty("responseName", "Forbidden")
                                    )
                                )
                        );
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        context.Response.WriteAsync(payload.ToString());

                        return Task.CompletedTask;
                    },

                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        var payload = new JObject
                        (
                            new JProperty("error",
                                new JObject(
                                    new JProperty("description", "You are unauthenticated to access"),
                                    new JProperty("responseCode", 401),
                                    new JProperty("responseName", "Unauthorized")
                                    )
                                )
                        );
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Response.WriteAsync(payload.ToString());

                        return Task.CompletedTask;
                    },

                    OnMessageReceived = context =>
                    {
                        if (!string.IsNullOrEmpty(context.Request.Headers[HeaderNames.Authorization].ToString()))
                        {
                            context.Token = context.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
                        }
                        else if (context.Request.Cookies[configuration.GetSection("TokenSettings")["CookieName"]] != null)
                        {
                            context.Token = context.Request.Cookies[configuration.GetSection("TokenSettings")["CookieName"]];
                        }
                        return Task.CompletedTask;
                    }
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.IncludeErrorDetails = true;
                options.GetTokenValidationParameters(configuration);
            });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .Build();
            });
        }
    }

}
