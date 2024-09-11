using BookMarketPlace.Services.BasketApi.ConfigurationSettings;
using BookMarketPlace.Services.BasketApi.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));


builder.Services.AddSingleton<RedisService>(opt =>
{
    var redisSettings=opt.GetRequiredService<IOptions<RedisSettings>>().Value;

    var redisService= new RedisService(redisSettings.Host, redisSettings.Port);

    redisService.Connect();

    return redisService;

});


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

app.UseAuthorization();

app.MapControllers();

app.Run();
