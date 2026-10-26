using Microsoft.EntityFrameworkCore;
using RoomBooking.Api.Infrastructure;

namespace RoomBooking.Api.Application.Room;

public class RoomService(AppDbContext db) : IRoomService
{
    public async Task<List<RoomDto>> GetAsync(RoomFilter filter)
    {
        var query = db.Rooms.AsNoTracking().AsQueryable();

        if (filter.MinCapacity is not null)
            query = query.Where(r => r.Capacity >= filter.MinCapacity);
        if (filter.Floor is not null)
            query = query.Where(r => r.Floor == filter.Floor);
        var ids = filter.EquipmentIds?.Distinct().ToList();
        if (ids is { Count: > 0 })
            query = query.Where(r => r.Equipments.Count(e => ids.Contains(e.Id)) == ids.Count);

        return await query
            .OrderBy(r => r.Name)
            .Select(r => new RoomDto(r.Id, r.Name, r.Capacity, r.Floor, r.IsActive,
                r.Equipments.Select(e => e.Name).ToList()))
            .ToListAsync();
    }
}