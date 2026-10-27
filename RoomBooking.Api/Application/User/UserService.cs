using Microsoft.EntityFrameworkCore;
using RoomBooking.Api.Domain;
using RoomBooking.Api.Infrastructure;

namespace RoomBooking.Api.Application.User;

public class UserService(AppDbContext db) : IUserService
{
    public async Task<List<UserDto>> GetAsync()
    {
        var query = db.Users.AsNoTracking().AsQueryable();

        return await query
            .OrderBy(u => u.Id)
            .Select(u => new UserDto(u.Id, u.FirstName, u.LastName,
                db.Reservations.Count(r => r.Organizer.Equals(u) && r.Status == ReservationStatus.Booked)))
            .ToListAsync();
    }
}