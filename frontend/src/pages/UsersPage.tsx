import { useEffect, useState } from 'react';
import { errorMessage, getUsers } from '../api';
import type { User } from '../types';

export function UsersPage() {
  const [users, setUsers] = useState<User[]>([]);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    let ignore = false;
    getUsers()
      .then((value) => { if (!ignore) setUsers(value); })
      .catch((e: unknown) => { if (!ignore) setError(errorMessage(e)); })
      .finally(() => { if (!ignore) setLoading(false); });
    return () => { ignore = true; };
  }, []);

  return (
    <section>
      <h2 className="h4 mb-3">Users</h2>
      {error && <div className="alert alert-danger" role="alert">{error}</div>}
      {loading ? <p>Loading users...</p> : (
        <div className="table-responsive">
          <table className="table table-striped align-middle">
            <thead><tr><th>ID</th><th>First name</th><th>Last name</th><th>Active reservations</th></tr></thead>
            <tbody>
              {users.map((user) => (
                <tr key={user.id}>
                  <td>{user.id}</td><td>{user.firstName}</td><td>{user.lastName}</td>
                  <td>{user.activeReservations}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </section>
  );
}
