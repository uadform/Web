using ItemStore.WebApi.Services;
using ItemStore.WebApi.Interfaces;
using Npgsql;
using System.Data;
using ItemStore.WebApi.Repositories;
using ItemStore.WebApi.Contexts;
using Microsoft.EntityFrameworkCore;
using ItemStore.WebApi.Middlewares;
using ItemStore.WebApi.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration["MySecrets:PostgreConnection"] ?? throw new ArgumentNullException("Connection string was not found."); ;
builder.Services.AddTransient<IDbConnection>(sp => new NpgsqlConnection(connectionString));


//builder.Services.AddDbContext<DataContext>(o => o.UseInMemoryDatabase("MyDatabase"));
builder.Services.AddDbContext<DataContext>(o => o.UseNpgsql(connectionString));

builder.Services.AddTransient<IItemService, ItemService>();
builder.Services.AddTransient<IEFCoreRepository, EFCoreRepository>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<JsonPlaceholderClient>();

builder.Services.AddHttpClient();
var app = builder.Build();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.InjectStylesheet("C:/Users/u.adomavicius/source/repos/Web/ItemStore.WebApi/Scripts/swagger.css");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
