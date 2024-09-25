using FlightEase.API.OdataConfiguration;
using FlightEaseDB.BusinessLogic.Generations.DependencyInjection;
using Microsoft.AspNetCore.OData;
using Repositories.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.InitializerDependencyInjection();
builder.Services.AddODataConfiguration();
builder.Services.DependencyDBInit();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
