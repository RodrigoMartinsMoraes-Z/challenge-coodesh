using FitnessFoodsLC.Context;
using FitnessFoodsLC.Domain.Products;
using FitnessFoodsLC.Interface;
using FitnessFoodsLC.Interface.Context;
using FitnessFoodsLC.Repository;
using FitnessFoodsLC.Service;

using Microsoft.EntityFrameworkCore;

using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { @"bin\" }, StringSplitOptions.None)[0];
IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(projectPath)
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOptions();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<IFitnessFoodsLCContext, FitnessFoodsLCContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString("FitnessFoodsLC"), b => b.MigrationsAssembly("FitnessFoodsLC.Context"));
    },
    ServiceLifetime.Scoped);
#region Interfaces

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOpenFoods, OpenFoods>();

#endregion

var app = builder.Build();

var baseUrl = configuration.GetValue<string>("BaseUrl");
#region CRON
var schedule = configuration.GetValue<string>("Schedule");

var cron = new ScheduledService(baseUrl, schedule);
var cancelationToken = new CancellationToken();
cron.StartAsync(cancelationToken);
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UsePathBase(baseUrl);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
