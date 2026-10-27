# RoomBooking

A full-stack web application for managing office meeting-room reservations. Users can browse and filter rooms, view organizers, book meetings, cancel upcoming reservations, end meetings early, and inspect room usage statistics.

This project was built for the **IDT internship developer technical assignment**.

## Tech stack

| Layer | Technologies |
| --- | --- |
| Backend | C#, ASP.NET Core Web API, .NET 10 |
| Data storage | Entity Framework Core InMemory provider |
| Frontend | React, TypeScript, Vite, Bootstrap |
| Testing | xUnit (backend booking-rule tests) |

There is **no authentication or authorization**, as specified in the assignment.

## Features

- **Rooms:** View room name, ID, capacity, floor, equipment and active/inactive status. Filter by minimum capacity, floor and equipment.
- **Users:** View users and their active-reservation counts.
- **Reservations:** View and filter reservations by room, organizer, status and date range. Create, cancel and end reservations early.
- **Validation:** Booking rules are enforced by the **backend**, with descriptive HTTP error responses that are shown in the UI.
- **Usage report:** Compare reserved hours with estimated used hours and flag potentially underused rooms.
- **Demo data:** Seeded rooms, users and reservations (including historical and in-progress meetings when using the expanded seeder).

## Getting started

### Prerequisites

- **.NET 10 SDK**
- **Node.js** compatible with the Vite version in `frontend/package.json` (for current Vite releases, use Node.js 20.19+ or 22.12+)
- **npm**

### 1. Clone the repository

```bash
git clone https://github.com/YOUR_USERNAME/RoomBooking.git
cd RoomBooking
```

Replace `YOUR_USERNAME` with the actual GitHub account name.

### 2. Start the backend

From the repository root:

```bash
dotnet restore
dotnet run --project RoomBooking.Api --launch-profile http
```

The API listens at **http://localhost:5077** using the checked-in HTTP launch profile.

Quick check: open <http://localhost:5077/api/rooms> in your browser. It should return a JSON array.

### 3. Start the frontend

In a **second terminal**, from the repository root:

```bash
cd frontend
npm install
npm run dev
```

Open the address printed by Vite (usually **http://localhost:5173**).

The development proxy in `frontend/vite.config.ts` forwards requests starting with `/api` to `http://localhost:5077`, so the frontend and backend can run on separate local ports without extra CORS configuration.

> If `frontend/` has not been initialized yet, create it with `npm create vite@latest frontend -- --template react-ts`, install dependencies including Bootstrap, then copy the provided React source and Vite configuration into it. The frontend files are designed to be placed **inside** the repository beside `RoomBooking.Api`.

### 4. Run the tests

From the repository root:

```bash
dotnet test
```

To check the production frontend build:

```bash
cd frontend
npm run build
```

## Project structure

```text
RoomBooking/
├── RoomBooking.Api/
│   ├── Application/
│   │   ├── Equipment/
│   │   ├── Reports/
│   │   ├── Reservation/
│   │   │   └── Rules/
│   │   ├── Room/
│   │   └── User/
│   ├── Controllers/
│   ├── Domain/
│   ├── Infrastructure/
│   │   ├── AppDbContext.cs
│   │   ├── DbSeeder.cs
│   │   └── BookingExceptionHandler.cs
│   ├── Program.cs
│   └── appsettings.json
├── RoomBooking.Tests/
│   └── Application/Reservation/Rules/
├── frontend/
│   └── src/
│       ├── api.ts
│       ├── types.ts
│       ├── pages/
│       │   ├── RoomsPage.tsx
│       │   ├── UsersPage.tsx
│       │   ├── ReservationsPage.tsx
│       │   ├── ReportsPage.tsx
│       │   └── reservations/
│       │       ├── ReservationForm.tsx
│       │       ├── ReservationFilters.tsx
│       │       └── ReservationsTable.tsx
│       └── App.tsx
└── README.md
```

### Architecture

The backend separates HTTP controllers from application services, domain entities and data access. Reservation validation uses independent `IBookingRule` implementations evaluated by `BookingValidator`. Services and rules are registered through dependency injection. API responses use DTOs rather than directly exposing EF Core entities.

The React frontend keeps API calls in `src/api.ts`, shared response types in `src/types.ts`, and UI components in `src/pages/`. The reservation page separates its form, filters and table from its API orchestration.

## Booking rules

Each new reservation must satisfy the following server-side rules:

| ID | Rule | Requirement |
| --- | --- | --- |
| R1 | Maximum duration | At most **7 days**, including exactly 7 days |
| R2 | Room overlap | No overlapping active reservations in the same room; cancelled bookings do not block a room |
| R3 | Organizer overlap | A user cannot organize overlapping meetings, even across different rooms |
| R4 | Valid time | Start must be in the future; end must be after start |
| R5 | Capacity | Number of attendees cannot exceed the room capacity |
| R6 | Booking horizon | Start cannot be more than **90 days** in the future |
| R7 | Room status | Inactive rooms cannot be booked |

The duration and booking-horizon limits are configurable in `RoomBooking.Api/appsettings.json`:

```json
"Booking": {
  "MaxDurationDays": 7,
  "MaxAdvanceDays": 90
}
```

> The backend is the source of truth for business rules. HTML form restrictions on the frontend are only for convenience.

## REST API

All endpoints are relative to `http://localhost:5077`.

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/rooms` | List/filter rooms (`MinCapacity`, `Floor`, `EquipmentIds`) |
| `GET` | `/api/equipments` | List available equipment |
| `GET` | `/api/users` | List users and active-reservation counts |
| `GET` | `/api/reservations` | List/filter reservations (`RoomId`, `OrganizerId`, `Status`, `FromDate`, `ToDate`) |
| `GET` | `/api/reservations/{id}` | Get reservation details |
| `POST` | `/api/reservations` | Create a reservation |
| `POST` | `/api/reservations/{id}/cancel` | Cancel an upcoming reservation |
| `POST` | `/api/reservations/{id}/end` | End an in-progress meeting early |
| `GET` | `/api/reports/room-usage` | Usage report (optional `From` and `To` dates) |

An example request body for `POST /api/reservations`:

```json
{
  "roomId": 1,
  "organizerId": 1,
  "title": "Team planning",
  "startDate": "2026-11-01T09:00:00Z",
  "endDate": "2026-11-01T10:00:00Z",
  "attendees": 5
}
```

**Adjust the dates** to a future period within the configured booking horizon before sending the request, and choose a time that does not conflict with existing bookings.

Validation failures return an HTTP error and a problem response with a human-readable `detail` and a machine-readable `code`, for example:

```json
{
  "title": "Booking rule violated",
  "status": 409,
  "detail": "Only a meeting that is in progress can be ended early.",
  "code": "CANNOT_END"
}
```

## Usage reporting

`GET /api/reports/room-usage` calculates per-room figures such as total reservations, completed and cancelled counts, early endings, booked/used hours and utilization percentage. Report thresholds are configured under `Report` in `appsettings.json`:

```json
"Report": {
  "LowUtilizationPercent": 50,
  "MinReservationsForFlag": 3
}
```

The report uses reservation timestamps and recorded early endings as a **proxy** for actual room usage. It does not track attendees checking in, so its `UsedHours` number is an estimate rather than verified occupancy.

The frontend Reservations page reloads its list after changes and automatically every 30 seconds. The Reports page loads its data when opened or when the report period changes; it does not poll automatically.

## Data and limitations

- **In-memory database:** Data is seeded at API startup and is **not persisted** across backend restarts. This is intentional for the assignment.
- **Cancel / end:** A reservation may be cancelled before it starts; only a meeting currently in progress can be ended early.
- **Editing:** Updating an existing reservation (`PUT`) is not implemented; the required create/cancel/end actions are provided.
- **Time zones:** The UI accepts local time and converts it to UTC for the API. Dates are displayed using the viewer's local timezone.
- **No authentication:** All pages and API endpoints are accessible without a login, as requested in the assignment.

## GitHub

Use `.gitignore` to exclude generated `.NET` directories (`bin/`, `obj/`), frontend dependencies (`node_modules/`), build output (`dist/`) and local configuration/secrets. Commit the source files, `frontend/package.json` and the lockfile (when present).

---

Built as an internship technical task to demonstrate full-stack development, maintainable application architecture and server-side business validation.
