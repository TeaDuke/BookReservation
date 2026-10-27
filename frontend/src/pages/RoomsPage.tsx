import { useEffect, useState } from 'react';
import { errorMessage, getEquipment, getRooms } from '../api';
import type { Equipment, Room } from '../types';

export function RoomsPage() {
  const [rooms, setRooms] = useState<Room[]>([]);
  const [equipment, setEquipment] = useState<Equipment[]>([]);
  const [minimumCapacity, setMinimumCapacity] = useState('');
  const [floor, setFloor] = useState('');
  const [equipmentId, setEquipmentId] = useState('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    getEquipment().then(setEquipment).catch((e: unknown) => setError(errorMessage(e)));
  }, []);

  useEffect(() => {
    let ignore = false;
    const params = new URLSearchParams();
    if (minimumCapacity) params.set('MinCapacity', minimumCapacity);
    if (floor) params.set('Floor', floor);
    if (equipmentId) params.append('EquipmentIds', equipmentId);

    getRooms(params)
      .then((result) => {
        if (!ignore) { setRooms(result); setError(''); }
      })
      .catch((e: unknown) => { if (!ignore) setError(errorMessage(e)); })
      .finally(() => { if (!ignore) setLoading(false); });

    return () => { ignore = true; };
  }, [minimumCapacity, floor, equipmentId]);

  return (
    <section>
      <h2 className="h4 mb-3">Meeting rooms</h2>
      <div className="row g-3 mb-4">
        <div className="col-sm-4">
          <label className="form-label" htmlFor="room-min-capacity">Minimum capacity</label>
          <input id="room-min-capacity" type="number" min="1" className="form-control"
            value={minimumCapacity} onChange={(e) => setMinimumCapacity(e.target.value)} />
        </div>
        <div className="col-sm-4">
          <label className="form-label" htmlFor="room-floor">Floor</label>
          <input id="room-floor" type="number" className="form-control"
            value={floor} onChange={(e) => setFloor(e.target.value)} />
        </div>
        <div className="col-sm-4">
          <label className="form-label" htmlFor="room-equipment">Equipment</label>
          <select id="room-equipment" className="form-select" value={equipmentId}
            onChange={(e) => setEquipmentId(e.target.value)}>
            <option value="">All equipment</option>
            {equipment.map((item) => <option key={item.id} value={item.id}>{item.name}</option>)}
          </select>
        </div>
      </div>
      {error && <div className="alert alert-danger" role="alert">{error}</div>}
      {loading && <p>Loading rooms...</p>}
      {!loading && (
        <div className="table-responsive">
          <table className="table table-striped align-middle">
            <thead><tr><th>ID</th><th>Name</th><th>Capacity</th><th>Floor</th><th>Equipment</th><th>Status</th></tr></thead>
            <tbody>
              {rooms.map((room) => (
                <tr key={room.id}>
                  <td>{room.id}</td><td>{room.name}</td><td>{room.capacity}</td>
                  <td>{room.floor}</td><td>{room.equipments.join(', ') || '—'}</td>
                  <td><span className={`badge ${room.isActive ? 'bg-success' : 'bg-secondary'}`}>
                    {room.isActive ? 'Active' : 'Inactive'}
                  </span></td>
                </tr>
              ))}
            </tbody>
          </table>
          {rooms.length === 0 && <p>No rooms match your filters.</p>}
        </div>
      )}
    </section>
  );
}
