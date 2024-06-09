using BookMarketPlace.Services.CatalogApi.ConfigurationDbSettings;
using BookMarketPlace.Services.CatalogApi.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();

builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddSingleton<IDbSettings>(x =>
{
    return x.GetRequiredService<IOptions<DbSettings>>().Value;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddAutoMapper(typeof(Program));

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
