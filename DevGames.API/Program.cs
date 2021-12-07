using DevGames.API.Mappers;
using DevGames.API.Persistence;
using DevGames.API.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
//{
//    var settings = config.Build();
//    Serilog.Log.Logger = new LoggerConfiguration()
//        .Enrich.FromLogContext()
//        .WriteTo.MSSqlServer(settings.GetConnectionString("DevGamesCs"),
//        sinkOptions: new MSSqlServerSinkOptions()
//        {
//            AutoCreateSqlTable = true,
//            TableName = "Logs"
//        })
//        .CreateLogger();
//}).UseSerilog();

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DevGamesCs");

//builder.Services
//    .AddDbContext<DevGamesContext>(o => 
//        o.UseSqlServer(connectionString));

builder.Services
    .AddDbContext<DevGamesContext>(o =>
        o.UseInMemoryDatabase("DevGamesDb"));

builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();

builder.Services.AddAutoMapper(typeof(BoardMapper));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DevGames API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "LuisDev",
            Email = "contato@luisdev.com.br",
            Url = new Uri("https://luisdev.com.br")
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
