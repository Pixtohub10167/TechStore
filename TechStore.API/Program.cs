using Microsoft.EntityFrameworkCore;
using TechStore.BLL.Interfaces;
using TechStore.BLL.Services;
using TechStore.DAL.Context;
using TechStore.DAL.Interfaces;
using TechStore.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// --- Слой доступа к данным ---
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// --- Слой бизнес-логики ---
builder.Services.AddScoped<IOrderService, OrderService>();

// --- Слой представления ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
