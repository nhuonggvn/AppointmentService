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

            // 1. Merge và patch động các file ocelot.*.json từ biến môi trường
            MergeAndPatchOcelotConfig(builder.Environment.ContentRootPath);
            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

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

        private static void MergeAndPatchOcelotConfig(string rootPath)
        {
            var appHost = Environment.GetEnvironmentVariable("APPOINTMENT_SERVICE_HOST") ?? "26.88.31.108";
            var appScheme = Environment.GetEnvironmentVariable("APPOINTMENT_SERVICE_SCHEME") ?? "http";
            var appPort = Environment.GetEnvironmentVariable("APPOINTMENT_SERVICE_PORT") ?? "5000";

            var medHost = Environment.GetEnvironmentVariable("MEDICAL_SERVICE_HOST") ?? "26.79.10.201";
            var medScheme = Environment.GetEnvironmentVariable("MEDICAL_SERVICE_SCHEME") ?? "http";
            var medPort = Environment.GetEnvironmentVariable("MEDICAL_SERVICE_PORT") ?? "5000";

            var pharmHost = Environment.GetEnvironmentVariable("PHARMACY_SERVICE_HOST") ?? "26.71.15.204";
            var pharmScheme = Environment.GetEnvironmentVariable("PHARMACY_SERVICE_SCHEME") ?? "http";
            var pharmPort = Environment.GetEnvironmentVariable("PHARMACY_SERVICE_PORT") ?? "5000";

            var gatewayBaseUrl = Environment.GetEnvironmentVariable("GATEWAY_BASE_URL") ?? "http://localhost:8000";

            var routeSegments = new List<string>();
            var globalConfigSegment = $"{{\n    \"BaseUrl\": \"{gatewayBaseUrl}\"\n  }}";

            var files = Directory.GetFiles(rootPath, "ocelot.*.json");
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file).ToLower();
                if (fileName == "ocelot.json" || fileName == "ocelot.global.json") continue;

                var content = File.ReadAllText(file);
                
                if (fileName.Contains("appointment"))
                {
                    content = content.Replace("\"Host\": \"26.88.31.108\"", $"\"Host\": \"{appHost}\"")
                                     .Replace("\"Port\": 5000", $"\"Port\": {appPort}")
                                     .Replace("\"DownstreamScheme\": \"http\"", $"\"DownstreamScheme\": \"{appScheme}\"");
                }
                else if (fileName.Contains("medical"))
                {
                    content = content.Replace("\"Host\": \"26.79.10.201\"", $"\"Host\": \"{medHost}\"")
                                     .Replace("\"Port\": 5000", $"\"Port\": {medPort}")
                                     .Replace("\"DownstreamScheme\": \"http\"", $"\"DownstreamScheme\": \"{medScheme}\"");
                }
                else if (fileName.Contains("pharmacy"))
                {
                    content = content.Replace("\"Host\": \"26.71.15.204\"", $"\"Host\": \"{pharmHost}\"")
                                     .Replace("\"Port\": 5000", $"\"Port\": {pharmPort}")
                                     .Replace("\"DownstreamScheme\": \"http\"", $"\"DownstreamScheme\": \"{pharmScheme}\"");
                }

                var routesStart = content.IndexOf("\"Routes\":");
                if (routesStart != -1)
                {
                    var arrayStart = content.IndexOf("[", routesStart);
                    var arrayEnd = content.LastIndexOf("]");
                    if (arrayStart != -1 && arrayEnd != -1 && arrayEnd > arrayStart)
                    {
                        var routesContent = content.Substring(arrayStart + 1, arrayEnd - arrayStart - 1).Trim();
                        if (!string.IsNullOrEmpty(routesContent))
                        {
                            routeSegments.Add(routesContent);
                        }
                    }
                }
            }

            var mergedJson = $"{{\n  \"Routes\": [\n{string.Join(",\n", routeSegments)}\n  ],\n  \"GlobalConfiguration\": {globalConfigSegment}\n}}";
            File.WriteAllText(Path.Combine(rootPath, "ocelot.json"), mergedJson);
        }
    }
}
