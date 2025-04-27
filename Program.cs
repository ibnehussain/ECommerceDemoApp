using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;

var builder = WebApplication.CreateBuilder(args);

// Read from app settings
var connectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];

// Register OpenTelemetry tracing and register Tracer as Singleton
builder.Services.AddSingleton(sp =>
{
    var tracerProvider = Sdk.CreateTracerProviderBuilder()
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("ECommerceDemoApp"))
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddAzureMonitorTraceExporter(o =>
        {
            o.ConnectionString = connectionString;
        })
        .Build();

    return tracerProvider.GetTracer("ECommerceDemoAppTracer");
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
