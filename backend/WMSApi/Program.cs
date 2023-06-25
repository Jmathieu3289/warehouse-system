using Microsoft.EntityFrameworkCore;
using WMSApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add contexts to the container.
builder.Services.AddDbContext<ItemContext>(opt =>opt.UseInMemoryDatabase("WMS"));
builder.Services.AddDbContext<PalletContext>(opt =>opt.UseInMemoryDatabase("WMS"));
builder.Services.AddDbContext<PalletBayContext>(opt =>opt.UseInMemoryDatabase("WMS"));
builder.Services.AddDbContext<PurchaseOrderContext>(opt =>opt.UseInMemoryDatabase("WMS"));
builder.Services.AddDbContext<PurchaseOrderItemContext>(opt =>opt.UseInMemoryDatabase("WMS"));
builder.Services.AddDbContext<SalesOrderContext>(opt =>opt.UseInMemoryDatabase("WMS"));
builder.Services.AddDbContext<SalesOrderItemContext>(opt =>opt.UseInMemoryDatabase("WMS"));

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
