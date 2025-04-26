using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Azure.Monitor.OpenTelemetry.Exporter;

var builder = WebApplication.CreateBuilder(args);

// Configure OpenTelemetry
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService("ECommerceDemoApp"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddAzureMonitorTraceExporter(options =>
            {
                options.ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
            });
    });

builder.Services.AddControllersWithViews();
// Add this temporary debug code before builder.Build():
var configValue = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
Console.WriteLine($"AI Connection String: {(string.IsNullOrEmpty(configValue) ? "NOT FOUND" : "CONFIGURED")}");
//
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapDefaultControllerRoute();
app.Run();