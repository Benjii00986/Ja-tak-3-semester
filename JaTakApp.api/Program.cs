using JaTak.DataAccess.Connections;
using JaTak.DataAccess.Repositories;
using JaTak.BusinessLogic.Services;

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

// Registers the offer service with scoped lifetime.
builder.Services.AddScoped<OfferService>();

// Registers services for API documentation and Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Enables authorization middleware.
app.UseAuthorization();

// Maps controller endpoints to the request pipeline.
app.MapControllers();

// Starts the application.
app.Run();
