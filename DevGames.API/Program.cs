using DevGames.API.Mappers;
using DevGames.API.Persistence;
using DevGames.API.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DevGamesCs");

builder.Services
    .AddDbContext<DevGamesContext>(o => 
        o.UseSqlServer(connectionString));

//builder.Services
//    .AddDbContext<DevGamesContext>(o =>
//        o.UseInMemoryDatabase("DevGamesDb"));

builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();

builder.Services.AddAutoMapper(typeof(BoardMapper));

builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
