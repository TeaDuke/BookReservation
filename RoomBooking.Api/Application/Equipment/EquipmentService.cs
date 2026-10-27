using Microsoft.EntityFrameworkCore;
using RoomBooking.Api.Infrastructure;

namespace RoomBooking.Api.Application.Equipment;

public class EquipmentService(AppDbContext db) : IEquipmentService
{
    public async Task<List<EquipmentDto>> GetAsync()
    {
        var query = db.Equipments.AsNoTracking().AsQueryable();

        return await query
            .OrderBy(e => e.Name)
            .Select(e => new EquipmentDto(e.Id, e.Name, e.Rooms.Select(r => r.Name).ToList()))
            .ToListAsync();
    }
}