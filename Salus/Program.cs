//models
global using Salus.Models;
global using Salus.Models.Requests;
global using Salus.Models.Enums;
global using static Salus.Models.Enums.RecipeEnums;
global using static Salus.Models.Enums.UserProfileEnums;
//services
global using Salus.Services.AuthServices;
global using Salus.Services.RecipeServices;
global using Salus.Services.Last24hServices;
global using Salus.Services.SocialMediaServices;
global using Salus.Services.TagServices;
global using Salus.Services.UserProfileServices;
global using Salus.Services.Interfaces;

global using Microsoft.AspNetCore.Mvc;
global using System.ComponentModel.DataAnnotations;
global using Salus.Data;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Authorization;
global using System.Security.Claims;
global using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Salus.Services;
using System.Reflection;
using System.Reflection.Emit;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDietService, DietService>();
builder.Services.AddScoped<ILast24hService, Last24hService>();
builder.Services.AddScoped<IOilService, OilService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<ISocialMediaService, SocialMediaService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Start Authorization header using the Bearer scheme (\"bearer {token}\"!)",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(builder.Configuration.GetSection("Keys:JwtKey").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddMvc()
     .AddNewtonsoftJson(
          options => {
              options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
          });
builder.Services.AddDbContext<DataContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(o => o
//later use "WithOrigin("www.something.com")"
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

