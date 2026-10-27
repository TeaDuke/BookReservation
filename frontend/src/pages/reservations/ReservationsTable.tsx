import { formatDate } from '../../api';
import type { Reservation } from '../../types';

export type ReservationAction = 'cancel' | 'end';

interface Props {
  reservations: Reservation[];
  loading: boolean;
  pendingId: number | null;
  onAction: (id: number, action: ReservationAction) => void;
}

export function ReservationsTable({ reservations, loading, pendingId, onAction }: Props) {
  const now = Date.now();

  return (
    <section aria-label="Reservation list">
      <div className="table-responsive">
        <table className="table table-striped align-middle">
          <thead>
            <tr>
              <th>ID</th><th>Room</th><th>Organizer</th><th>Title</th>
              <th>Start</th><th>End</th><th>Attendees</th><th>Status</th><th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {reservations.map((reservation) => {
              const start = new Date(reservation.startDate).getTime();
              const end = new Date(reservation.endDate).getTime();
              const booked = reservation.status === 'Booked';
              const canCancel = booked && start > now;
              const canEnd = booked && start <= now && now < end;

              return (
                <tr key={reservation.id}>
                  <td>{reservation.id}</td>
                  <td>{reservation.room.name}</td>
                  <td>{reservation.organizer.firstName} {reservation.organizer.lastName}</td>
                  <td>{reservation.title}</td>
                  <td>{formatDate(reservation.startDate)}</td>
                  <td>{formatDate(reservation.endDate)}</td>
                  <td>{reservation.attendees}</td>
                  <td>{reservation.status}</td>
                  <td>
                    <div className="d-flex gap-1">
                      {canCancel && (
                        <button type="button" className="btn btn-outline-danger btn-sm"
                          disabled={pendingId !== null}
                          onClick={() => onAction(reservation.id, 'cancel')}>
                          {pendingId === reservation.id ? 'Saving...' : 'Cancel'}
                        </button>
                      )}
                      {canEnd && (
                        <button type="button" className="btn btn-outline-primary btn-sm"
                          disabled={pendingId !== null}
                          onClick={() => onAction(reservation.id, 'end')}>
                          {pendingId === reservation.id ? 'Saving...' : 'End early'}
                        </button>
                      )}
                    </div>
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
      {!loading && reservations.length === 0 && (
        <p className="text-secondary">No reservations match your filters.</p>
      )}
    </section>
  );
}
