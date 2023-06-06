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
using Microsoft.AspNetCore.ResponseCompression;
using WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebApi.Requrements;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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


builder.Services.AddResponseCompression(x =>
{
    x.Providers.Clear();
    x.Providers.Add<GzipCompressionProvider>();
    x.Providers.Add<BrotliCompressionProvider>();

    //kompresja dla https domyœlnie jest wy³¹czona
    x.EnableForHttps = true;
});

builder.Services.Configure<GzipCompressionProviderOptions>(x => x.Level = System.IO.Compression.CompressionLevel.Optimal);
builder.Services.Configure<BrotliCompressionProviderOptions>(x => x.Level = System.IO.Compression.CompressionLevel.Fastest);


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



builder.Services.AddTransient<AuthService>();
builder.Services.AddSingleton<IAuthorizationHandler, KnownMailHandler>();
builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy("knownMailDomain", policy => policy.AddRequirements(new KnownMailRequirement("bb.cc")));
    //alternatywa
    options.AddPolicy("knownMailDomain", policy => policy/*.RequireClaim(ClaimTypes.Email, "aa@bb.cc")*/
                                                        .RequireAssertion((handler) => 
                                                        handler.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Email)?.Value.EndsWith("bb.cc") ?? false));
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(AuthService.KEY),
            ValidateIssuerSigningKey = true,

            //wy³¹czamy walidacjê (domyœlnie w³¹czona)
            ValidateIssuer = false,
            //sprawdzenie zgodnoœci Audience
            /*ValidateAudience = false*/
        };
        //options.Authority = "zxc";
        options.Audience = "abc";
    });


var app = builder.Build();


app.UseResponseCompression();

app.UseAuthentication();
app.UseAuthorization();

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
