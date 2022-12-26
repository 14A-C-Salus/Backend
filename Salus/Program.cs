//models
global using Salus.Controllers.Models.AuthModels;
global using Salus.Controllers.Models.FoodModels;
global using Salus.Controllers.Models.JoiningEntity;
global using Salus.Controllers.Models.Last24hModels;
global using Salus.Controllers.Models.RecipeModels;
global using Salus.Controllers.Models.SocialMediaModels;
global using Salus.Controllers.Models.TagModels;
global using Salus.Controllers.Models.UserProfileModels;

//services
global using Salus.Services.AuthServices;
global using Salus.Services.FoodServices;
global using Salus.Services.Last24hServices;
global using Salus.Services.RecipeServices;
global using Salus.Services.SocialMediaServices;
global using Salus.Services.TagServices;
global using Salus.Services.UserProfileServices;

//templates
global using Salus.Templates;

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
using Swashbuckle.AspNetCore.Filters;
using Newtonsoft.Json;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<ILast24hService, Last24hService>();
builder.Services.AddScoped<IOilService, OilService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<ISocialMediaService, SocialMediaService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();

builder.Services.AddHttpContextAccessor();
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

