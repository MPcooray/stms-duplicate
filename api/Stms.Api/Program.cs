using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Stms.Api.Data;
using Stms.Api.Models;
using Stms.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// DB: try DefaultConnection -> Default -> DB_CONNECTION (env)
// ---------------------------------------------------------
var cs =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? builder.Configuration.GetConnectionString("Default")
    ?? builder.Configuration["DB_CONNECTION"];

if (string.IsNullOrWhiteSpace(cs))
    throw new InvalidOperationException(
        "No DB connection string found. Set ConnectionStrings:DefaultConnection or ConnectionStrings:Default " +
        "in appsettings.json (or set DB_CONNECTION env var).");

// MariaDB/MySQL
builder.Services.AddDbContext<StmsDbContext>(opt =>
    opt.UseMySql(cs, ServerVersion.AutoDetect(cs)));

// ---------------------------------------------------------
// JWT Auth (fail fast if config missing)
// ---------------------------------------------------------
string? jwtIssuer   = builder.Configuration["JWT:Issuer"];
string? jwtAudience = builder.Configuration["JWT:Audience"];
string? jwtKey      = builder.Configuration["JWT:Key"];

if (string.IsNullOrWhiteSpace(jwtIssuer) ||
    string.IsNullOrWhiteSpace(jwtAudience) ||
    string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "Missing JWT config. Please set JWT:Issuer, JWT:Audience and JWT:Key in appsettings.json or environment.");
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
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

// ---------------------------------------------------------
// Services (your DI registrations)
// ---------------------------------------------------------
builder.Services.AddScoped<IRankingService, RankingService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();
builder.Services.AddScoped<IUniversityService, UniversityService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IResultService, ResultService>();
builder.Services.AddScoped<ITournamentService, TournamentService>();
builder.Services.AddScoped<ResultService>(); // not needed if you inject IResultService

builder.Services.AddControllers();
builder.Services.AddAuthorization();

// CORS for Vite UI
builder.Services.AddCors(o =>
    o.AddPolicy("ui", p =>
        p.WithOrigins("http://localhost:5173")
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials()));

// Swagger + Bearer button
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "STMS API", Version = "v1" });

    var scheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Type: Bearer {your JWT token}"
    };

    c.AddSecurityDefinition("Bearer", scheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ui");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ---------------------------------------------------------
// DB migrate + seed admin
// ---------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StmsDbContext>();
    db.Database.Migrate(); // apply migrations

    if (!db.Users.Any())
    {
        db.Users.Add(new User
        {
            Email = "admin@stms.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin#123"),
            Role = "Admin"
        });
        db.SaveChanges();
    }
}

app.MapGet("/", () => "STMS API running");

// ---------------------------------------------------------
// Minimal login endpoint
// ---------------------------------------------------------
app.MapPost("/auth/login", (StmsDbContext db, LoginDto dto) =>
{
    var user = db.Users.FirstOrDefault(u => u.Email == dto.Email);
    if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        return Results.Unauthorized();

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(ClaimTypes.Role, user.Role),
    };

    var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var jwt = new JwtSecurityToken(
        issuer: jwtIssuer,
        audience: jwtAudience,
        claims: claims,
        expires: DateTime.UtcNow.AddHours(8),
        signingCredentials: creds);

    return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(jwt) });
});

app.Run();

record LoginDto(string Email, string Password);
