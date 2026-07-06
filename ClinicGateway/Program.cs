using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ClinicGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Add Ocelot JSON Configuration and merge all ocelot.*.json files
            builder.Configuration.AddOcelot(builder.Environment);

            // 2. Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // 3. Register JWT Authentication (shared secret key)
            var secretKey = builder.Configuration["Jwt:Secret"] ?? "ThisIsMySuperSecretKeyForJwtTokenDoNotShareIt12345";
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ClockSkew = TimeSpan.Zero
                };
            });

            // 4. Add Ocelot Services
            builder.Services.AddOcelot(builder.Configuration);

            var app = builder.Build();

            // Configure HTTP Request Pipeline
            app.UseCors("AllowAll");

            // Ocelot works with Authentication if configured
            app.UseAuthentication();
            app.UseAuthorization();

            // Run Ocelot Middleware
            app.UseOcelot().Wait();

            app.Run();
        }
    }
}
