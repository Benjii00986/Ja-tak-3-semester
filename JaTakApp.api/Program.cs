using JaTak.DataAccess.Connections;
using JaTak.DataAccess.Repositories;
using JaTak.BusinessLogic.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration
    .GetConnectionString("JaTakDatabase")
    ?? throw new InvalidOperationException(
        "Connection string 'JaTakDatabase' was not found.");

builder.Services.AddSingleton(
    new SqlConnectionFactory(connectionString));

builder.Services.AddScoped<OfferRepository>();

builder.Services.AddScoped<OfferService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
