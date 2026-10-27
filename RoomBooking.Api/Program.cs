using System.Text.Json.Serialization;
using RoomBooking.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Api.Application.Equipment;
using RoomBooking.Api.Application.Reservation;
using RoomBooking.Api.Application.Reservation.Rules;
using RoomBooking.Api.Application.Room;
using RoomBooking.Api.Application.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<BookingOptions>(builder.Configuration.GetSection("Booking"));
builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddScoped<IBookingRule, ValidStartTimeRule>();
builder.Services.AddScoped<IBookingRule, ValidTimeRangeRule>();
builder.Services.AddScoped<IBookingRule, MaxDurationRule>();
builder.Services.AddScoped<IBookingRule, MaxCapacityRule>();
builder.Services.AddScoped<IBookingRule, BookingHorizonRule>();
builder.Services.AddScoped<IBookingRule, ActiveRoomRule>();
builder.Services.AddScoped<IBookingRule, RoomOverlapRule>();
builder.Services.AddScoped<IBookingRule, OrganizerOverlapRule>();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseInMemoryDatabase("RoomBooking"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(db);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
