using backend.Data;
using backend.Jobs;
using backend.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure Entity Framework Core
builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfire(x =>
    x.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<WeatherService>(); // Registers HttpClient for API calls
builder.Services.AddScoped<WeatherService>();     // Registers WeatherService
builder.Services.AddScoped<WeatherFetchJob>();    // Registers WeatherFetchJob

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:7235") // Allow Vue.js frontend
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseHangfireDashboard(); //Will be available under http://localhost:5000/hangfire"
app.UseHangfireServer();


// Schedule to run every hour
RecurringJob.AddOrUpdate<WeatherFetchJob>("fetch-weather-data", job => job.ExecuteAsync(), Cron.Hourly);

app.UseCors("AllowVueFrontend");

app.UseAuthorization();


app.MapControllers();



app.Run();
