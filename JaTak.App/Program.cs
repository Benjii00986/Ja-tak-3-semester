// Creates the application builder and loads configuration.
var builder = WebApplication.CreateBuilder(args);

// Registers a named HTTP client for communicating with the JaTak Web API.
builder.Services.AddHttpClient("JaTakApi", client =>
{
    // Sets the base URL of the API using the application configuration.
    client.BaseAddress = new Uri(
        builder.Configuration["ApiSettings:BaseUrl"]!);
});

// Registers MVC controllers and views.
builder.Services.AddControllersWithViews();

// Builds the application.
var app = builder.Build();

// Configures error handling and security for production environments.
if (!app.Environment.IsDevelopment())
{
    // Redirects unhandled exceptions to the error page.
    app.UseExceptionHandler("/Home/Error");

    // Enables HTTP Strict Transport Security (HSTS).
    app.UseHsts();
}

// Redirects HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Enables routing for incoming requests.
app.UseRouting();

// Enables authorization middleware.
app.UseAuthorization();

// Maps static assets such as CSS, JavaScript and images.
app.MapStaticAssets();

// Configures the default MVC route.
// If no controller or action is specified, the Offers controller's Index action is used.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Offers}/{action=Index}/{id?}")
    .WithStaticAssets();

// Starts the application.
app.Run();