import { useState } from 'react';
import type { FormEvent } from 'react';
import { toUtcIso } from '../../api';
import type { CreateReservationRequest, Room, User } from '../../types';

interface ReservationFormValues {
  title: string;
  roomId: string;
  organizerId: string;
  startDate: string;
  endDate: string;
  attendees: string;
}

const emptyForm: ReservationFormValues = {
  title: '',
  roomId: '',
  organizerId: '',
  startDate: '',
  endDate: '',
  attendees: '1',
};

interface Props {
  rooms: Room[];
  users: User[];
  onCreate: (request: CreateReservationRequest) => Promise<boolean>;
}

export function ReservationForm({ rooms, users, onCreate }: Props) {
  const [form, setForm] = useState<ReservationFormValues>(emptyForm);
  const [saving, setSaving] = useState(false);

  function update<K extends keyof ReservationFormValues>(
    field: K,
    value: ReservationFormValues[K],
  ) {
    setForm((previous) => ({ ...previous, [field]: value }));
  }

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    if (saving) return;

    setSaving(true);
    try {
      const wasCreated = await onCreate({
        title: form.title.trim(),
        roomId: Number(form.roomId),
        organizerId: Number(form.organizerId),
        startDate: toUtcIso(form.startDate),
        endDate: toUtcIso(form.endDate),
        attendees: Number(form.attendees),
      });
      if (wasCreated) {
        setForm((previous) => ({ ...previous, title: '' }));
      }
    } finally {
      setSaving(false);
    }
  }

  return (
    <form className="card p-3 mb-4" onSubmit={(e) => { void handleSubmit(e); }}>
      <h3 className="h5">New reservation</h3>
      <fieldset disabled={saving}>
        <div className="row g-3">
          <div className="col-md-6">
            <label htmlFor="title" className="form-label">Meeting title</label>
            <input id="title" className="form-control" maxLength={200} required
              value={form.title} onChange={(e) => update('title', e.target.value)} />
          </div>
          <div className="col-md-3">
            <label htmlFor="book-room" className="form-label">Room</label>
            <select id="book-room" className="form-select" required value={form.roomId}
              onChange={(e) => update('roomId', e.target.value)}>
              <option value="">Choose room</option>
              {rooms.filter((room) => room.isActive).map((room) => (
                <option key={room.id} value={room.id}>
                  {room.name} (max {room.capacity})
                </option>
              ))}
            </select>
          </div>
          <div className="col-md-3">
            <label htmlFor="book-user" className="form-label">Organizer</label>
            <select id="book-user" className="form-select" required value={form.organizerId}
              onChange={(e) => update('organizerId', e.target.value)}>
              <option value="">Choose user</option>
              {users.map((user) => (
                <option key={user.id} value={user.id}>{user.firstName} {user.lastName}</option>
              ))}
            </select>
          </div>
          <div className="col-md-4">
            <label htmlFor="book-start" className="form-label">Start (local time)</label>
            <input id="book-start" type="datetime-local" className="form-control" required
              value={form.startDate} onChange={(e) => update('startDate', e.target.value)} />
          </div>
          <div className="col-md-4">
            <label htmlFor="book-end" className="form-label">End (local time)</label>
            <input id="book-end" type="datetime-local" className="form-control" required
              value={form.endDate} onChange={(e) => update('endDate', e.target.value)} />
          </div>
          <div className="col-md-4">
            <label htmlFor="book-attendees" className="form-label">Attendees</label>
            <input id="book-attendees" type="number" min="1" step="1"
              className="form-control" required value={form.attendees}
              onChange={(e) => update('attendees', e.target.value)} />
          </div>
        </div>
        <div className="mt-3">
          <button className="btn btn-primary" type="submit">
            {saving ? 'Saving...' : 'Book room'}
          </button>
        </div>
      </fieldset>
    </form>
  );
}
