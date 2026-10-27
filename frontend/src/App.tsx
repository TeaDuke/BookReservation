import { useState } from 'react';
import { RoomsPage } from './pages/RoomsPage';
import { UsersPage } from './pages/UsersPage';
import { ReservationsPage } from './pages/ReservationsPage';
import { ReportsPage } from './pages/ReportsPage';

type Page = 'rooms' | 'users' | 'reservations' | 'reports';

const pages: { id: Page; title: string }[] = [
  { id: 'rooms', title: 'Rooms' },
  { id: 'users', title: 'Users' },
  { id: 'reservations', title: 'Reservations' },
  { id: 'reports', title: 'Usage report' },
];

export default function App() {
  const [page, setPage] = useState<Page>('rooms');

  return (
    <main className="container py-4">
      <header className="mb-4">
        <h1 className="h2">RoomBooking</h1>
        <p className="text-secondary mb-3">Office meeting room reservation system</p>
        <nav className="d-flex flex-wrap gap-2" aria-label="Main navigation">
          {pages.map((item) => (
            <button
              key={item.id}
              type="button"
              className={`btn ${page === item.id ? 'btn-primary' : 'btn-outline-primary'}`}
              onClick={() => setPage(item.id)}
            >
              {item.title}
            </button>
          ))}
        </nav>
      </header>
      {page === 'rooms' && <RoomsPage />}
      {page === 'users' && <UsersPage />}
      {page === 'reservations' && <ReservationsPage />}
      {page === 'reports' && <ReportsPage />}
    </main>
  );
}
