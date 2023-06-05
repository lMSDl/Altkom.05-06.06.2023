using Bogus;
using Models;
using Services.Bogus;
using Services.Bogus.Fakers;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddSingleton<ICrudService<User>, CrudService<User>>();
builder.Services.AddTransient<BaseFaker<User>, UserFaker>();



var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();

app.Run();
