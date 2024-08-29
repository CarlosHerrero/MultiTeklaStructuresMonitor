using Grpc.Net.Client;

using Microsoft.Extensions.DependencyInjection;

using MultiTeklaStructuresMonitor.Components;
using MultiTeklaStructuresMonitor.Interop;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

#pragma warning disable ASP0000 // this is ok now
using (var serviceProvider = builder.Services.BuildServiceProvider())
{
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
    var driversSink = GrpcServiceSink.CreateInstance(logger);
    builder.Services.AddSingleton(driversSink);
}


// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// configure the service sink drivers


app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
