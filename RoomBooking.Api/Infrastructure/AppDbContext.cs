using Microsoft.EntityFrameworkCore;
using RoomBooking.Api.Domain;

namespace RoomBooking.Api.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Equipment> Equipments => Set<Equipment>();
}