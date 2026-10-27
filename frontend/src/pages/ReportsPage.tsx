import { useEffect, useState } from 'react';
import { errorMessage, formatDate, getRoomUsage, toUtcIso } from '../api';
import type { RoomUsageReport } from '../types';

export function ReportsPage() {
  const [report, setReport] = useState<RoomUsageReport | null>(null);
  const [from, setFrom] = useState('');
  const [to, setTo] = useState('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    let ignore = false;
    const params = new URLSearchParams();
    if (from) params.set('From', toUtcIso(from));
    if (to) params.set('To', toUtcIso(to));
    setLoading(true);
    getRoomUsage(params)
      .then((result) => { if (!ignore) { setReport(result); setError(''); } })
      .catch((e: unknown) => { if (!ignore) setError(errorMessage(e)); })
      .finally(() => { if (!ignore) setLoading(false); });
    return () => { ignore = true; };
  }, [from, to]);

  return (
    <section>
      <h2 className="h4 mb-3">Room usage report</h2>
      <p className="text-secondary">Based on completed, cancelled and early-ended meetings. Without attendance tracking, actual usage is estimated.</p>
      <div className="row g-3 mb-3">
        <div className="col-sm-6">
          <label className="form-label" htmlFor="report-from">From (optional)</label>
          <input id="report-from" type="datetime-local" className="form-control" value={from}
            onChange={(e) => setFrom(e.target.value)} />
        </div>
        <div className="col-sm-6">
          <label className="form-label" htmlFor="report-to">To (optional)</label>
          <input id="report-to" type="datetime-local" className="form-control" value={to}
            onChange={(e) => setTo(e.target.value)} />
        </div>
      </div>
      {error && <div className="alert alert-danger" role="alert">{error}</div>}
      {loading && <p>Loading report...</p>}
      {!loading && report && (
        <>
          <p className="text-secondary">Period: {formatDate(report.from)} — {formatDate(report.to)}</p>
          <div className="table-responsive">
            <table className="table table-striped align-middle">
              <thead><tr><th>Room</th><th>Floor</th><th>Reservations</th><th>Completed</th><th>Cancelled</th><th>Ended early</th><th>Booked hours</th><th>Estimated used hours</th><th>Unused hours</th><th>Utilization</th><th>Flag</th></tr></thead>
              <tbody>
                {report.rooms.map((room) => (
                  <tr key={room.roomId}>
                    <td>{room.roomName}</td><td>{room.floor}</td>
                    <td>{room.totalReservations}</td><td>{room.completedCount}</td>
                    <td>{room.cancelledCount}</td><td>{room.endedEarlyCount}</td>
                    <td>{room.bookedHours}</td><td>{room.usedHours}</td><td>{room.unusedHours}</td>
                    <td>{room.utilizationPercent}%</td>
                    <td>{room.isUnderused ? <span className="badge bg-warning text-dark">Underused</span> : '—'}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </>
      )}
    </section>
  );
}
