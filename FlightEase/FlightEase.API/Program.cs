using FlightEase.API.OdataConfiguration;
using FlightEaseDB.BusinessLogic.Generations.DependencyInjection;
using Microsoft.AspNetCore.OData;
using Microsoft.OpenApi.Models;
using Repositories.Repositories.BaseRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FlightEase API", Version = "v1" });
});
builder.Services.InitializerDependencyInjection();
builder.Services.AddODataConfiguration();
builder.Services.DependencyDBInit();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();
app.UseRouting();


app.Run();
