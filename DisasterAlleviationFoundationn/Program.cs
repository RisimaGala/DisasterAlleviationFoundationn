using DisasterAlleviationFoundationn.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

// Add detailed logging
var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database - used SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(true);
    options.EnableDetailedErrors(true);
});

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Database setup with detailed logging
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation(" STARTING DATABASE SETUP...");

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        logger.LogInformation("Checking database connection...");
        var canConnect = await context.Database.CanConnectAsync();
        logger.LogInformation($"Database can connect: {canConnect}");

        if (!canConnect)
        {
            logger.LogInformation("Creating database...");
            await context.Database.EnsureCreatedAsync();
            logger.LogInformation(" DATABASE CREATED SUCCESSFULLY");
        }
        else
        {
            logger.LogInformation("Database already exists");
        }

        // Testing if i can actually use the database
        var userCount = await context.Users.CountAsync();
        logger.LogInformation($" DATABASE TEST: {userCount} users found");

        logger.LogInformation(" DATABASE SETUP COMPLETED");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, " DATABASE SETUP FAILED");
        logger.LogError($"Error details: {ex.Message}");
        if (ex.InnerException != null)
        {
            logger.LogError($"Inner error: {ex.InnerException.Message}");
        }
    }
}

await app.RunAsync();