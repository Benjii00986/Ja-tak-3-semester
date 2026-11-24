using JaTak.DataAccess.Connections;
using JaTak.DataAccess.Repositories;
using JaTak.BusinessLogic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

// Creates the application builder and loads configuration.
var builder = WebApplication.CreateBuilder(args);

// Retrieves the database connection string from the application configuration.
var connectionString = builder.Configuration
    .GetConnectionString("JaTakDatabase")
    ?? throw new InvalidOperationException(
        "Connection string 'JaTakDatabase' was not found.");

// Registers the SQL connection factory as a singleton.
builder.Services.AddSingleton(
    new SqlConnectionFactory(connectionString));

// Registers the repository with scoped lifetime.
builder.Services.AddScoped<OfferRepository>();

// Registers the JaTak user repository with scoped lifetime.
builder.Services.AddScoped<JaTakUserRepository>();

// Registers the offer service with scoped lifetime.
builder.Services.AddScoped<OfferService>();

// Registers the JaTak user service with scoped lifetime.
builder.Services.AddScoped<JaTakUserService>();

// Registers the reservation repository with scoped lifetime.
builder.Services.AddScoped<ReservationRepository>();

// Registers the reservation service with scoped lifetime.
builder.Services.AddScoped<ReservationService>();

// Registers services for API documentation and Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configures JWT authentication using Keycloak.
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:8080/realms/JaTak";
        options.Audience = "jatak-api";

        options.MapInboundClaims = false;

        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "http://localhost:8080/realms/JaTak",

            ValidateAudience = true,
            ValidAudience = "jatak-api",

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

// Registers authorization services.
builder.Services.AddAuthorization();

// Registers controllers for handling HTTP requests.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Builds the application.
var app = builder.Build();

// Enables Swagger and Swagger UI in the development environment.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Redirects HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Validates incoming authentication tokens.
app.UseAuthentication();

// Handles authorization requirements.
app.UseAuthorization();

// Maps controller endpoints to the request pipeline.
app.MapControllers();

// Starts the application.
app.Run();
