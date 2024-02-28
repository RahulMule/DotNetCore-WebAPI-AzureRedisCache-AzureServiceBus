using DotNetCore_WebAPI_AzureRedisCache.IRepository;
using DotNetCore_WebAPI_AzureRedisCache.ProductContext;
using DotNetCore_WebAPI_AzureRedisCache.Repository;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var redis = builder.Configuration.GetConnectionString("connstring");
builder.Services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(redis));
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseInMemoryDatabase("Products"));
builder.Services.AddScoped<IProduct,ProductRepository>();
builder.Services.AddScoped<ICache, AzureRedisCache>();

builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
