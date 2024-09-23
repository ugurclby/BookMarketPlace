using BookMarketPlace.Core.Services;
using BookMarketPlace.Services.BasketApi.ConfigurationSettings;
using BookMarketPlace.Services.BasketApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

var requiredAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration.GetSection("IdentityServerUrl").Value;
    opt.Audience = "resource_basket";
    opt.RequireHttpsMetadata = false;
});

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers(configure =>
{
    configure.Filters.Add(new AuthorizeFilter(requiredAuthorizePolicy));
});
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));


builder.Services.AddSingleton<RedisService>(opt =>
{
    var redisSettings=opt.GetRequiredService<IOptions<RedisSettings>>().Value;

    var redisService= new RedisService(redisSettings.Host, redisSettings.Port);

    redisService.Connect();

    return redisService;

});

builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
builder.Services.AddScoped<IBasketService, BasketService>();
 


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
