using Bogus;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Bogus;
using Services.Bogus.Fakers;
using Services.Interfaces;
using System.Diagnostics;
using FluentValidation;
using FluentValidation.AspNetCore;

using System.Text.Json.Serialization;
using WebApi.Validators;
using WebApi.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddFluentValidation()
                /*.AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.IgnoreReadOnlyProperties= true;
                    x.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
                    x.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.Strict;
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });*/
                .AddNewtonsoftJson(x =>
                {
                    x.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    x.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
                    //x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    x.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
                    //x.SerializerSettings.DateFormatString = "yyy-MM-d_ff:ss;mm";
                    x.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.None;
                });

//builder.Services.AddFluentValidationAutoValidation(x => x.);
builder.Services.AddTransient<IValidator<Product>, ProductValidator>();

//wy³¹czenie automatycznej walidacji modelu
builder.Services.Configure<ApiBehaviorOptions>(x => x.SuppressModelStateInvalidFilter = true);


builder.Services.AddSingleton<IUsersService, UsersService>();
builder.Services.AddTransient<BaseFaker<User>, UserFaker>();

builder.Services.AddSingleton<ICrudService<Product>, CrudService<Product>>();
builder.Services.AddTransient<BaseFaker<Product>, ProductFaker>();

builder.Services.AddTransient<ConsoleLogFilter>();
builder.Services.AddSingleton(x => new LimitFilter(5));

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
