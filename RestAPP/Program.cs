using System.Reflection;
using Microsoft.OpenApi.Models;
using RestAPI.Context;
using RestAPI.Interfaces;
using RestAPI.Service;
using RestAPP.Context;
using RestAPP.Interfaces;
using RestAPP.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Регистрация сервисов
builder.Services.AddScoped<IAdministrator, AdministratorService>();
builder.Services.AddScoped<IBooking, BookingService>();
builder.Services.AddScoped<IDish, DishService>();
builder.Services.AddScoped<IRestaurant, RestaurantService>();
builder.Services.AddScoped<IRestaurantDish, RestaurantDishService>();
builder.Services.AddScoped<IUser, UserService>();

// Регистрация контекстов
builder.Services.AddScoped<AdminContext>();
builder.Services.AddScoped<BookingContext>();
builder.Services.AddScoped<DishContext>();
builder.Services.AddScoped<RestaurantContext>();
builder.Services.AddScoped<RestaurantDishContext>();
builder.Services.AddScoped<UserContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RestAPI",
        Version = "v1",
        Description = "Методы в контроллере"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestAPI v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();