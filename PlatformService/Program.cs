using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

var configurations = builder.Configuration;
var environment = builder.Environment;

// Add services to the container.

if(environment.IsProduction())
{
    Console.WriteLine("--> Using Production DB");

    builder.Services.AddDbContext<AppDBContext>(opt => 
        opt.UseSqlServer(configurations.GetConnectionString("PlatformsConnectionString")));

    builder.Services.AddDbContext<AppDBContext>(opt => 
        opt.UseInMemoryDatabase("InMem"));
}
else
{
    Console.WriteLine("--> Using InMem Developmemtn DB");

    builder.Services.AddDbContext<AppDBContext>(opt => 
        opt.UseInMemoryDatabase("InMem"));
}

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

PrepDB.PrepPopulateion(app, environment.IsProduction());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
