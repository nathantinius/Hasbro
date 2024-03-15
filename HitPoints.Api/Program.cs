using FluentValidation;
using HitPoints.Api;
using HitPoints.Api.Mapping;
using HitPoints.Api.SeedData;
using HitPoints.Application;
using HitPoints.Application.Database;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddDatabase(config["Database:ConnectionString"]!);
builder.Services.AddSingleton<Seed>();

builder.Services.AddValidatorsFromAssemblyContaining<IApiMarker>(ServiceLifetime.Singleton);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ValidationMappingMiddleware>();

app.MapControllers();

var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
await dbInitializer.InitializeAsync();

var seedData = app.Services.GetRequiredService<Seed>();
await seedData.Execute();

app.Run();
