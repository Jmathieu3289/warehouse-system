using System.Configuration;
using Microsoft.EntityFrameworkCore;
using WMSApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add contexts to the container.
var configuration = builder.Configuration;
builder.Services.AddDbContext<ApplicationContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        //c.InjectStylesheet("http://localhost:4200/swagger-style.css")
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
