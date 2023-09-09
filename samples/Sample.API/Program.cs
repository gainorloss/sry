using Microsoft.EntityFrameworkCore;
using Sample.API.Core;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AdminDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenTelemetry(builder.Configuration, $"{builder.Environment.ApplicationName}.{builder.Environment.EnvironmentName}");
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(builder => builder
.WithOrigins("http://localhost:3000", "http://localhost:7000", "http://localhost:7002")
.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials());
app.UseAuthorization();
app.MapEnvironments();
app.MapControllers();

app.Run();
