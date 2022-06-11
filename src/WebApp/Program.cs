using Microsoft.AspNetCore.ResponseCompression;
using Paladin.Api.Sdk;
using Paladin.WebApp.Pages.Environments;

var builder = WebApplication.CreateBuilder(args);

///////////////////////
// Configure services.
///////////////////////
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddPaladinApiClient();

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.AddHostedService<RefreshBackgroundService>();

///////////////////////
// Configure app.
///////////////////////
var app = builder.Build();

app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapHub<EnvironmentHub>($"/{nameof(EnvironmentHub)}");
});

app.Run();
