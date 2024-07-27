using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using ThreadApi.Common.Config;
using ThreadApi.Common.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(new SerilogJsonFormatter(), "logs/log-.json", rollingInterval: RollingInterval.Month)
    .CreateLogger();



builder.Host.UseSerilog();

// Add services to the container.
// Add services for controllers and API versioning
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DatabaseService>(); // Add DatabaseService
builder.Services.AddApiVersioningConfiguration(); // Add API versioning

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware to log all requests and responses
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); // Top-level route registration for controllers

app.Run();

