using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPP.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AdminContext>(options =>
    options.UseSqlServer(DbConnection.config));

builder.Services.AddDbContext<BookingContext>(options =>
    options.UseSqlServer(DbConnection.config));

builder.Services.AddDbContext<DishContext>(options =>
    options.UseSqlServer(DbConnection.config));

builder.Services.AddDbContext<RestaurantContext>(options =>
    options.UseSqlServer(DbConnection.config));

builder.Services.AddDbContext<RestaurantDishContext>(options =>
    options.UseSqlServer(DbConnection.config));

builder.Services.AddDbContext<UserContext>(options =>
    options.UseSqlServer(DbConnection.config));

builder.Services.AddScoped<RestAPI.Interfaces.IAdministrator, RestAPI.Service.AdministratorService>();
builder.Services.AddScoped<RestAPI.Interfaces.IBooking, RestAPI.Service.BookingService>();
builder.Services.AddScoped<RestAPI.Interfaces.IDish, RestAPI.Service.DishService>();
builder.Services.AddScoped<RestAPI.Interfaces.IRestaurant, RestAPI.Service.RestaurantService>();
builder.Services.AddScoped<RestAPI.Interfaces.IRestaurantDish, RestAPI.Service.RestaurantDishService>();
builder.Services.AddScoped<RestAPI.Interfaces.IUser, RestAPI.Service.UserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
