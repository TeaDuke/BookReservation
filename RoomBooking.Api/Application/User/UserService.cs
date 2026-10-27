using Microsoft.EntityFrameworkCore;
using RoomBooking.Api.Domain;
using RoomBooking.Api.Infrastructure;

namespace RoomBooking.Api.Application.User;

public class UserService(AppDbContext db, TimeProvider time) : IUserService
{
    public async Task<List<UserDto>> GetAsync()
    {
        var now = time.GetUtcNow().UtcDateTime;
        
        var query = db.Users.AsNoTracking().AsQueryable();

        return await query
            .OrderBy(u => u.Id)
            .Select(u => new UserDto(u.Id, u.FirstName, u.LastName,
                db.Reservations.Count(r => r.Organizer.Equals(u) 
                                           && r.Status == ReservationStatus.Booked && r.EndDate > now )))
            .ToListAsync();
    }
}