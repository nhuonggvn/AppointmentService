using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using AppointmentService.Infrastructure.Data;
using AppointmentService.Infrastructure.Repositories.Implementations;
using AppointmentService.Infrastructure.Repositories.Interfaces;

namespace AppointmentService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Services to the Container

            // 1. Add DbContext
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppointmentDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("AppointmentService.API")));

            // 2. Add Generic Repository
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // 3. Add Controllers
            builder.Services.AddControllers();
            builder.Services.AddHealthChecks();

            // 4. Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // 5. Add JWT Authentication
            var secretKey = builder.Configuration["Jwt:Secret"] ?? "ThisIsMySuperSecretKeyForJwtTokenDoNotShareIt12345";
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
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

            // 6. Add Swagger with JWT Support
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "BTL Fullstack Group 5 - Appointment Service API", 
                    Version = "v1",
                    Description = "API dich vu dat lich kham va quan ly hang cho (Appointment Service)"
                });

                // Add JWT Security Definition
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Dien JWT Token theo dang: Bearer {token}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            // Configure HTTP request pipeline

            // Always enable Swagger UI for students testing & demo
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Appointment Service API v1");
                c.RoutePrefix = string.Empty; // Serve Swagger at the root URL
            });

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHealthChecks("/health");

            // Auto-initialize & Seed Database với cơ chế retry (quan trọng khi chạy Docker)
            var retryCount = 10;
            var delayBetweenRetriesSeconds = 3;
            var dbSeeded = false;

            for (int i = 1; i <= retryCount; i++)
            {
                try
                {
                    using var scope = app.Services.CreateScope();
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<AppointmentDbContext>();

                    Console.WriteLine($"Dang ket noi de khoi tao va seed database (Lan thu {i}/{retryCount})...");
                    DbInitializer.Initialize(context);

                    Console.WriteLine("Khoi tao va seed database thanh cong.");
                    dbSeeded = true;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lan thu {i} that bai: {ex.Message}");
                    if (i < retryCount)
                    {
                        Console.WriteLine($"Cho {delayBetweenRetriesSeconds} giay truoc khi thu lai...");
                        Thread.Sleep(TimeSpan.FromSeconds(delayBetweenRetriesSeconds));
                    }
                }
            }

            if (!dbSeeded)
            {
                Console.WriteLine("CANH BAO: Khong the khoi tao va seed database sau nhieu lan thu. Ung dung co the khong hoat dong dung.");
            }

            app.Run();
        }
    }
}
