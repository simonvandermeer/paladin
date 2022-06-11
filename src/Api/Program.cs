using Microsoft.AspNetCore.HttpLogging;
using Paladin.Api.Environments;
using Paladin.Api.Environments.Simulated;

var builder = WebApplication.CreateBuilder(args);

///////////////////////
// Configure services.
///////////////////////
builder.Services.AddSingleton<ISimulatedEnvironmentRepository, InMemorySimulatedEnvironmentRepository>();
builder.Services.AddSingleton<EnvironmentService>();

builder.Services.AddControllers();

// Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Fixes exceptions caused by having multiple DTO's with the same class name.
    options.CustomSchemaIds(type => type.ToString());
});

// Logging.
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
});
builder.Logging.AddConsole();
builder.Logging.AddDebug();

///////////////////////
// Configure app.
///////////////////////
var app = builder.Build();

app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
