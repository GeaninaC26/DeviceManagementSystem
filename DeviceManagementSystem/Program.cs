using Microsoft.Data.SqlClient;
using DeviceManagementSystem.Application;
using DeviceManagementSystem.Infrastructure;
using DeviceManagementSystem.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException("JWT key is missing from configuration.");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.FromMinutes(2)
        };
    });

builder.Services.AddAuthorization(
    options =>
    {
        options.AddPolicy("Admin", policy =>
        {
            policy.RequireRole("Admin");
        });
    }
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Device Management System API",
            Description = "API for managing devices and user-device assignments"
        });
    }
);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll", policy =>
    {
        policy.AllowAnyMethod()
              .AllowAnyHeader()
              .WithOrigins(builder.Configuration.GetValue<string>("ApplicationSettings:ClientUrl"));
    });
});

var app = builder.Build();

app.UseGlobalExceptionHandler();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.Run();

