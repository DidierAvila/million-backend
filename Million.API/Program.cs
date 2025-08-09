using Microsoft.EntityFrameworkCore;
using Million.Application.Interfaces;
using Million.Infrastructure.Services;
using Million.Infrastructure.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Use in-memory database for development/testing
builder.Services.AddDbContext<MillionDbContext>(options =>
    options.UseInMemoryDatabase("MillionDb"));

// Register application services
builder.Services.AddScoped<IPropiedadService, PropiedadService>();
builder.Services.AddScoped<IPropietarioService, PropietarioService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
