using MassTransit;
using MedicalRecordService.Api.Middleware;
using MedicalRecordService.Api.Services.Implementations;
using MedicalRecordService.Api.Events;
using MedicalRecordService.Api.Services.Interfaces;
using MedicalRecordService.Api.Services.External;
using MedicalRecordService.Api.Helpers;
using MedicalRecordService.Data.DbContext;
using MedicalRecordService.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 2. Swagger with JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Medical Record Service API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token"
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

// 3. JWT Authentication (validate only)
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret is missing"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

// 4. Database Context
builder.Services.AddDbContext<MedicalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MedicalDB")));

// 5. Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// 6. Services (Business Logic)
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService.Api.Services.Implementations.MedicalRecordService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();

// 7. External Clients (Pharmacy Service)
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<TokenRelayHandler>();

builder.Services.AddHttpClient<IPharmacyServiceClient, PharmacyServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["PharmacyService:BaseUrl"] ?? "http://pharmacy-service");
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddHttpMessageHandler<TokenRelayHandler>();

// 8. MassTransit (RabbitMQ) for Events
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
        var username = builder.Configuration["RabbitMQ:Username"] ?? "guest";
        var password = builder.Configuration["RabbitMQ:Password"] ?? "guest";

        cfg.Host(host, "/", h =>
        {
            h.Username(username);
            h.Password(password);
        });

        // Cấu hình exchange name cho PrescriptionCreatedEvent
        cfg.Message<PrescriptionCreatedEvent>(m => m.SetEntityName("prescription.created"));

        cfg.ConfigureEndpoints(context);
    });
});

// 9. AutoMapper (if using)
builder.Services.AddAutoMapper(typeof(Program));

// 10. CORS (allow frontend)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// 11. Configure the HTTP request pipeline
app.Use(async (context, next) =>
{
    Console.WriteLine($"[DEBUG] Request Path: {context.Request.Method} {context.Request.Path}");
    await next();
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Medical Record Service v1");
});

// 12. Global error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// 13. Auto migrate database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MedicalDbContext>();
    db.Database.Migrate();
}

app.Run();