using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args); 

builder.Host.ConfigureAppConfiguration((hostBuilder, config) =>
{
    config.AddJsonFile($"configuration.{hostBuilder.HostingEnvironment.EnvironmentName.ToLower()}.json")
    .AddEnvironmentVariables();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("GatewayAuthenticationKey",opt =>
{
    opt.Authority = builder.Configuration.GetSection("IdentityServerUrl").Value;
    opt.Audience = "resource_gateway";
    opt.RequireHttpsMetadata = false;
});

builder.Services.AddOcelot();  

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.UseOcelot();

app.UseAuthorization(); 

app.Run();
