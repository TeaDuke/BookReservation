import type { ReservationStatus, Room, User } from '../../types';

export interface ReservationFilterValues {
  roomId: string;
  organizerId: string;
  status: ReservationStatus | '';
  from: string;
  to: string;
}

export const emptyReservationFilters: ReservationFilterValues = {
  roomId: '',
  organizerId: '',
  status: '',
  from: '',
  to: '',
};

interface Props {
  rooms: Room[];
  users: User[];
  filters: ReservationFilterValues;
  onChange: (next: ReservationFilterValues) => void;
}

export function ReservationFilters({ rooms, users, filters, onChange }: Props) {
  function update<K extends keyof ReservationFilterValues>(
    field: K,
    value: ReservationFilterValues[K],
  ) {
    onChange({ ...filters, [field]: value });
  }

  return (
    <section className="mb-4" aria-label="Reservation filters">
      <div className="d-flex justify-content-between align-items-center mb-2">
        <h3 className="h5 mb-0">Filter reservations</h3>
        <button type="button" className="btn btn-outline-secondary btn-sm"
          onClick={() => onChange({ ...emptyReservationFilters })}>
          Clear filters
        </button>
      </div>
      <div className="row g-2">
        <div className="col-lg-2 col-sm-6">
          <label htmlFor="filter-room" className="form-label">Room</label>
          <select id="filter-room" className="form-select" value={filters.roomId}
            onChange={(e) => update('roomId', e.target.value)}>
            <option value="">All rooms</option>
            {rooms.map((room) => <option key={room.id} value={room.id}>{room.name}</option>)}
          </select>
        </div>
        <div className="col-lg-2 col-sm-6">
          <label htmlFor="filter-organizer" className="form-label">Organizer</label>
          <select id="filter-organizer" className="form-select" value={filters.organizerId}
            onChange={(e) => update('organizerId', e.target.value)}>
            <option value="">All users</option>
            {users.map((user) => (
              <option key={user.id} value={user.id}>{user.firstName} {user.lastName}</option>
            ))}
          </select>
        </div>
        <div className="col-lg-2 col-sm-6">
          <label htmlFor="filter-status" className="form-label">Status</label>
          <select id="filter-status" className="form-select" value={filters.status}
            onChange={(e) => update('status', e.target.value as ReservationStatus | '')}>
            <option value="">All statuses</option>
            <option value="Booked">Booked</option>
            <option value="Completed">Completed</option>
            <option value="Cancelled">Cancelled</option>
          </select>
        </div>
        <div className="col-lg-3 col-sm-6">
          <label htmlFor="filter-from" className="form-label">From (local time)</label>
          <input id="filter-from" type="datetime-local" className="form-control"
            value={filters.from} onChange={(e) => update('from', e.target.value)} />
        </div>
        <div className="col-lg-3 col-sm-6">
          <label htmlFor="filter-to" className="form-label">To (local time)</label>
          <input id="filter-to" type="datetime-local" className="form-control"
            value={filters.to} onChange={(e) => update('to', e.target.value)} />
        </div>
      </div>
    </section>
  );
}
