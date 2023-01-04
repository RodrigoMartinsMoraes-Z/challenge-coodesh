using FitnessFoodsLC.Context;
using FitnessFoodsLC.Interface.Context;

using Microsoft.EntityFrameworkCore;

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
        options.UseNpgsql(configuration.GetConnectionString("FitnessFoodsLC"), b => b.MigrationsAssembly("FitnessFoodsLC.WebAPI"));
    },
    ServiceLifetime.Scoped);

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
