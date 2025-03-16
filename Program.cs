/*
    A food delivery backend trial program. Written by Rivo Aidla.
    Took a total of 18 hours of work in 3 days. Written in C# using Visual Studio 2022.
    All of my comments and code will be in English. It's much easier for me to explain things this way. 

    The project utilizes a total of 7 packages.
    Notable ones are Entity Framework (sqlite and tools) and Cronos (for Cronjob schedule formatting).

    Most scripts have been placed into their approprite directories for cleanliness.
    Those that aren't I haven't figured out a notable folder name for them to sit in.

    All keys, starting values and/or resources are taken from appsettings.json.
    For development, there is a appsettings.Development.json. It mostly just makes sure that
    the database used is the dev database. The default dso.db should stay clean for production.

    The project can be built using the build_rs.bat (Release build) or build_ds.bat (Debug build).
    The built project expects the user to have .NET Runtime 8 or later already installed.
*/

using Microsoft.EntityFrameworkCore;
using FoodDeliveryBackend.Data;
using FoodDeliveryBackend;

// Let's start out with the basics, this creates a builder for the web app.
var builder = WebApplication.CreateBuilder(args);

// The server's admin key for "SecureApi" functions.
var _secret_key = builder.Configuration["Secrets:AdminKey"];

// Load environment variables
builder.Configuration.AddEnvironmentVariables();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=dso.db";

// Configure services
// Setup our database service (sqlite) using default connection values from appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddHttpClient();

// Adds a WeatherDataFetcher, it grabs the current weather data from a fixed url.
builder.Services.AddScoped<WeatherDataFetcher>();

// Creates a custom background process called WeatherCronJob, using a cron schedule, it will run the WeatherDataFetcher.
builder.Services.AddSingleton<IHostedService, WeatherCronJob>(sp =>
{
    var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
    
    // Gets the cron schedule string from appsettings.json.
    var configuration = builder.Configuration.GetSection("WeatherFetcher").GetValue("CronSchedule", "0 0 * * *");
    return new WeatherCronJob(scopeFactory, configuration);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Used for debugging API calls.
builder.Services.AddLogging();

var app = builder.Build();

// If the admin key doesn't exist OR still has the placeholder key, warn the user.
if (string.IsNullOrEmpty(_secret_key) || _secret_key == "your_secret_key")
{
    Console.WriteLine("The Admin secret key is not setup in appsettings.json. For your servers safety, please replace the default value and/or add your own unique secret key when used in production.");
}

// Store the ServiceProvider in ServiceLocator for other scripts
ServiceLocator.Provider = app.Services;

// Configure the server to run on a user set address and port on Production.
if (!app.Environment.IsDevelopment())
{
    var url = "http://" + builder.Configuration.GetSection("ServerSettings").GetValue("HostAddress", "localhost") + ":"
        + builder.Configuration.GetSection("ServerSettings").GetValue("Port", "5000");

    Console.WriteLine("Setting Hosting Address to " + url);
    app.Urls.Add(url);
}
else // Otherwise, we startup some default developer stuff.
{
    using (var scope = app.Services.CreateScope())
    {
        Console.WriteLine("Generating and migrating database...");
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }

    app.UseSwagger(); // Used for debugging API calls.
    app.UseSwaggerUI(); // Used for debugging API calls.
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();