using JaTak.App.Handlers;
using JaTak.App.Services;

// Creates the application builder and loads configuration.
var builder = WebApplication.CreateBuilder(args);

// Allows services to access the current HTTP request context.
builder.Services.AddHttpContextAccessor();

// Registers the Keycloak token handler.
builder.Services.AddTransient<KeycloakTokenHandler>();

// Registers the HTTP client with automatic Keycloak token forwarding.
builder.Services
    .AddHttpClient("JaTakApi", client =>
    {
        client.BaseAddress = new Uri(
            builder.Configuration["ApiSettings:BaseUrl"]!);
    })
    .AddHttpMessageHandler<KeycloakTokenHandler>();

// Configures authentication using cookies and OpenID Connect.
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "OpenIdConnect";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("OpenIdConnect", options =>
    {
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.ClientId = builder.Configuration["Keycloak:ClientId"];
        options.ClientSecret = builder.Configuration["Keycloak:ClientSecret"];

        options.ResponseType = "code";

        options.SaveTokens = true;

        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");

        // Development only: Keycloak currently uses HTTP locally.
        if (builder.Environment.IsDevelopment())
        {
            options.RequireHttpsMetadata = false;
        }
    });

// Registers authorization services.
builder.Services.AddAuthorization();

// Registers the user API service.
builder.Services.AddScoped<UserApiService>();

// Registers MVC controllers and views.
builder.Services.AddControllersWithViews();

// Builds the application.
var app = builder.Build();

// Configures error handling and security for production environments.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirects HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Enables routing for incoming requests.
app.UseRouting();

// Enables authentication.
app.UseAuthentication();

// Enables authorization.
app.UseAuthorization();

// Maps static assets such as CSS, JavaScript and images.
app.MapStaticAssets();

// Configures the default MVC route.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Offers}/{action=Index}/{id?}")
    .WithStaticAssets();

// Starts the application.
app.Run();