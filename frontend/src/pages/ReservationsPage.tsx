import { useEffect, useState } from 'react';
import {
  cancelReservation,
  createReservation,
  endReservation,
  errorMessage,
  getReservations,
  getRooms,
  getUsers,
  toUtcIso,
} from '../api';
import type { CreateReservationRequest, Reservation, Room, User } from '../types';
import { ReservationFilters, emptyReservationFilters } from './reservations/ReservationFilters';
import type { ReservationFilterValues } from './reservations/ReservationFilters';
import { ReservationForm } from './reservations/ReservationForm';
import { ReservationsTable } from './reservations/ReservationsTable';
import type { ReservationAction } from './reservations/ReservationsTable';

type Feedback = { kind: 'success' | 'danger'; text: string };

function buildQuery(filters: ReservationFilterValues): URLSearchParams {
  const params = new URLSearchParams();
  if (filters.roomId) params.set('RoomId', filters.roomId);
  if (filters.organizerId) params.set('OrganizerId', filters.organizerId);
  if (filters.status) params.set('Status', filters.status);
  if (filters.from) params.set('FromDate', toUtcIso(filters.from));
  if (filters.to) params.set('ToDate', toUtcIso(filters.to));
  return params;
}

export function ReservationsPage() {
  const [options, setOptions] = useState<{ rooms: Room[]; users: User[] }>({
    rooms: [], users: [],
  });
  const [filters, setFilters] = useState<ReservationFilterValues>(emptyReservationFilters);
  const [reservations, setReservations] = useState<Reservation[]>([]);
  const [loading, setLoading] = useState(true);
  const [fetchError, setFetchError] = useState('');
  const [feedback, setFeedback] = useState<Feedback | null>(null);
  const [pendingId, setPendingId] = useState<number | null>(null);
  const [refreshKey, setRefreshKey] = useState(0);

  // Fetch room/user options only once per mount. Ignore late responses after unmount.
  useEffect(() => {
    let ignore = false;
    Promise.all([getRooms(), getUsers()])
      .then(([rooms, users]) => {
        if (!ignore) setOptions({ rooms, users });
      })
      .catch((error: unknown) => {
        if (!ignore) setFeedback({ kind: 'danger', text: errorMessage(error) });
      });
    return () => { ignore = true; };
  }, []);

  // Re-fetch on filter changes, manual refresh and every 30 seconds.
  // 'ignore' prevents a slow, obsolete response from replacing newer results.
  useEffect(() => {
    let ignore = false;
    setLoading(true);
    setFetchError('');

    getReservations(buildQuery(filters))
      .then((items) => { if (!ignore) setReservations(items); })
      .catch((error: unknown) => {
        if (!ignore) setFetchError(errorMessage(error));
      })
      .finally(() => { if (!ignore) setLoading(false); });

    return () => { ignore = true; };
  }, [filters, refreshKey]);

  useEffect(() => {
    const id = window.setInterval(() => setRefreshKey((value) => value + 1), 30_000);
    return () => window.clearInterval(id);
  }, []);

  function reload() {
    setRefreshKey((value) => value + 1);
  }

  async function handleCreate(request: CreateReservationRequest): Promise<boolean> {
    setFeedback(null);
    try {
      await createReservation(request);
      setFeedback({ kind: 'success', text: 'Reservation created.' });
      reload();
      return true;
    } catch (error: unknown) {
      setFeedback({ kind: 'danger', text: errorMessage(error) });
      return false;
    }
  }

  async function handleAction(id: number, action: ReservationAction) {
    if (pendingId !== null) return;
    setPendingId(id);
    setFeedback(null);

    try {
      if (action === 'cancel') await cancelReservation(id);
      else await endReservation(id);
      setFeedback({
        kind: 'success',
        text: action === 'cancel' ? 'Reservation cancelled.' : 'Meeting ended early.',
      });
      reload();
    } catch (error: unknown) {
      setFeedback({ kind: 'danger', text: errorMessage(error) });
    } finally {
      setPendingId(null);
    }
  }

  return (
    <section>
      <div className="d-flex flex-wrap justify-content-between align-items-center gap-2 mb-3">
        <h2 className="h4 mb-0">Reservations</h2>
        <button type="button" className="btn btn-outline-secondary btn-sm" onClick={reload}>
          Refresh list
        </button>
      </div>

      {feedback && (
        <div className={`alert alert-${feedback.kind}`} role={feedback.kind === 'danger' ? 'alert' : 'status'}>
          {feedback.text}
        </div>
      )}

      <ReservationForm rooms={options.rooms} users={options.users} onCreate={handleCreate} />
      <ReservationFilters rooms={options.rooms} users={options.users}
        filters={filters} onChange={setFilters} />

      {fetchError && <div className="alert alert-danger" role="alert">{fetchError}</div>}
      {loading ? <p className="text-secondary">Loading reservations...</p> : (
        !fetchError && <ReservationsTable reservations={reservations} loading={loading}
          pendingId={pendingId} onAction={(id, action) => { void handleAction(id, action); }} />
      )}
      <p className="text-secondary small mt-2">The list refreshes automatically every 30 seconds.</p>
    </section>
  );
}
