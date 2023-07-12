using Microsoft.ApplicationInsights.Extensibility;
using Polly.Registry;
using Serilog;
using WebApiPolly.Infrastructure;
using WebApiPolly.Polly;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("PoliciesSettings.json");

builder.Logging.ClearProviders();

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
    telemetryConfiguration.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
    loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Events);
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddPollyPolicyRegistry(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecastexternal", (IPolicyRegistry<string> policyRegistry) =>
{
    Log.Information("Request in weather forecast controller");

    var connector = new Connector(policyRegistry);
    
    return connector.SendRequest();
})
.WithName("GetWeatherForecastExternal");

app.Run();