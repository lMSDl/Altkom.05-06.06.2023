using Bogus;
using Microsoft.AspNetCore.Diagnostics;
using Models;
using Services.Bogus;
using Services.Bogus.Fakers;
using Services.Interfaces;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddSingleton<ICrudService<User>, CrudService<User>>();
builder.Services.AddTransient<BaseFaker<User>, UserFaker>();



var app = builder.Build();

app.Use(async (context, next) =>
{
if (Activity.Current == null)
{
    Console.WriteLine($"{context.TraceIdentifier}");
}
else
{
    Console.WriteLine($"00-{Activity.Current.TraceId}-{Activity.Current.SpanId}-00");
}
    await next();
});

// Configure the HTTP request pipeline.

app.MapControllers();

app.Run();
