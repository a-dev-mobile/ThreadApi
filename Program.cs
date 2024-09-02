using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using ThreadApi.Common.Config;
using ThreadApi.Common.Services;
using ThreadApi.Features.MetricThread.Services;

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
builder.Services.AddSingleton<MetricThreadService>(); 
builder.Services.AddApiVersioningConfiguration(); 

// Configure CORS to allow all origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

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

// Use the CORS policy
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers(); // Top-level route registration for controllers

app.Run();

