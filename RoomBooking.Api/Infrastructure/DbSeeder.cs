using RoomBooking.Api.Domain;

namespace RoomBooking.Api.Infrastructure;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Rooms.Any())
            return;
        
        var now =  DateTime.UtcNow;
        
        var projector = new Equipment { Name = "Projector" };
        var tvScreen = new Equipment { Name = "TVScreen" };
        var videoConferencing = new Equipment { Name = "VideoConferencing" };
        var whiteboard = new Equipment { Name = "Whiteboard" };

        var m1 = new Room { Name = "M1", Capacity  = 8, Floor = 1,  IsActive = true, 
            Equipments = [projector, videoConferencing, whiteboard]};
        var m2 = new Room { Name = "M2", Capacity  = 12, Floor = 1,  IsActive = true, 
            Equipments = [projector, videoConferencing, tvScreen, whiteboard]};
        var m3 = new Room { Name = "M3", Capacity  = 6, Floor = 1,  IsActive = true, 
            Equipments = [tvScreen, whiteboard]};
        var m4 = new Room { Name = "M4", Capacity = 12, Floor = 2, IsActive = false,
            Equipments = [videoConferencing, projector] };

        var anna = new User { FirstName = "Anna", LastName = "Smith" };
        var rokas = new User { FirstName = "Rokas", LastName = "Benz" };
        var slava =  new User { FirstName = "Slava", LastName = "Hype" };

        var reservations = new[]
        {
            new Reservation
            {
                Room = m1, Organizer = anna, Title = "Spring planning",
                StartDate = now.AddDays(-3), EndDate = now.AddDays(-3).AddHours(2),
                Attendees = 6, Status = ReservationStatus.Completed
            },
            new Reservation
            {
                Room = m2, Organizer = rokas, Title = "Sync", StartDate = now.AddDays(-2),
                EndDate = now.AddDays(-2).AddHours(2), EndedAt = now.AddDays(-2).AddMinutes(20),
                Attendees = 3, Status = ReservationStatus.Completed
            },
            new Reservation
            {
                Room = m3, Organizer = slava, Title = "Retro", StartDate = now.AddDays(1),
                EndDate = now.AddDays(1).AddHours(1), Attendees = 4, Status = ReservationStatus.Cancelled
            },
            new Reservation
            {
                Room = m1, Organizer = slava, Title = "Demo", StartDate = now.AddDays(2),
                EndDate = now.AddDays(2).AddHours(1), Attendees = 7, Status = ReservationStatus.Booked
            },

            new Reservation
            {
                Room = m1, Organizer = rokas, Title = "Quarterly planning",
                StartDate = now.AddDays(-20), EndDate = now.AddDays(-20).AddHours(2),
                Attendees = 5, Status = ReservationStatus.Completed
            },
            new Reservation
            {
                Room = m2, Organizer = slava, Title = "Sales review",
                StartDate = now.AddDays(-15), EndDate = now.AddDays(-15).AddHours(3),
                EndedAt = now.AddDays(-15).AddMinutes(55),
                Attendees = 8, Status = ReservationStatus.Completed
            },
            new Reservation
            {
                Room = m3, Organizer = anna, Title = "Design workshop",
                StartDate = now.AddDays(-12), EndDate = now.AddDays(-12).AddMinutes(90),
                Attendees = 4, Status = ReservationStatus.Completed
            },
            new Reservation
            {
                Room = m1, Organizer = anna, Title = "Product discovery",
                StartDate = now.AddDays(-10), EndDate = now.AddDays(-10).AddMinutes(150),
                EndedAt = now.AddDays(-10).AddHours(1),
                Attendees = 6, Status = ReservationStatus.Completed
            },
            new Reservation
            {
                Room = m2, Organizer = rokas, Title = "Client sync",
                StartDate = now.AddDays(-8), EndDate = now.AddDays(-8).AddHours(1),
                Attendees = 6, Status = ReservationStatus.Completed
            },
            new Reservation
            {
                Room = m3, Organizer = slava, Title = "Team retrospective",
                StartDate = now.AddDays(-6), EndDate = now.AddDays(-6).AddHours(1),
                Attendees = 3, Status = ReservationStatus.Completed
            },
            new Reservation
            {
                Room = m1, Organizer = rokas, Title = "Cancelled offsite",
                StartDate = now.AddDays(-5), EndDate = now.AddDays(-5).AddHours(2),
                Attendees = 7, Status = ReservationStatus.Cancelled
            },
            new Reservation
            {
                Room = m2, Organizer = anna, Title = "Cancelled budget sync",
                StartDate = now.AddDays(-4), EndDate = now.AddDays(-4).AddMinutes(90),
                Attendees = 5, Status = ReservationStatus.Cancelled
            },

            new Reservation
            {
                Room = m1, Organizer = anna, Title = "Live sprint planning",
                StartDate = now.AddMinutes(-25), EndDate = now.AddHours(2),
                Attendees = 6, Status = ReservationStatus.Booked
            },
            new Reservation
            {
                Room = m2, Organizer = rokas, Title = "Live client meeting",
                StartDate = now.AddMinutes(-10), EndDate = now.AddMinutes(90),
                Attendees = 8, Status = ReservationStatus.Booked
            }
        };
        
        db.Equipments.AddRange(projector, tvScreen, videoConferencing, whiteboard);
        db.Rooms.AddRange(m1, m2, m3, m4);
        db.Users.AddRange(anna, rokas, slava);
        db.Reservations.AddRange(reservations);
        db.SaveChanges();
    }
}