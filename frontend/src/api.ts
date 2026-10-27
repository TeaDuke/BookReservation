import type {
  CreateReservationRequest,
  Equipment,
  Reservation,
  Room,
  RoomUsageReport,
  User,
} from './types';

type ApiProblem = {
  detail?: string;
  title?: string;
  code?: string;
  errors?: Record<string, string[]>;
};

async function request<T>(path: string, options: RequestInit = {}): Promise<T> {
  const response = await fetch(path, {
    ...options,
    headers: {
      ...(options.body ? { 'Content-Type': 'application/json' } : {}),
      ...options.headers,
    },
  });

  if (!response.ok) {
    const body = await response.text();
    let problem: ApiProblem = {};
    try {
      problem = JSON.parse(body) as ApiProblem;
    } catch {
      problem.detail = body;
    }

    const validationErrors = problem.errors
      ? Object.values(problem.errors).flat().join(' ')
      : undefined;
    throw new Error(
      problem.detail || validationErrors || problem.title || `HTTP ${response.status}`,
    );
  }

  return (await response.json()) as T;
}

export function getRooms(params = new URLSearchParams()): Promise<Room[]> {
  return request<Room[]>(`/api/rooms?${params.toString()}`);
}

export function getEquipment(): Promise<Equipment[]> {
  return request<Equipment[]>('/api/equipments');
}

export function getUsers(): Promise<User[]> {
  return request<User[]>('/api/users');
}

export function getReservations(params = new URLSearchParams()): Promise<Reservation[]> {
  return request<Reservation[]>(`/api/reservations?${params.toString()}`);
}

export function createReservation(data: CreateReservationRequest): Promise<Reservation> {
  return request<Reservation>('/api/reservations', {
    method: 'POST',
    body: JSON.stringify(data),
  });
}

export function cancelReservation(id: number): Promise<Reservation> {
  return request<Reservation>(`/api/reservations/${id}/cancel`, { method: 'POST' });
}

export function endReservation(id: number): Promise<Reservation> {
  return request<Reservation>(`/api/reservations/${id}/end`, { method: 'POST' });
}

export function getRoomUsage(params = new URLSearchParams()): Promise<RoomUsageReport> {
  return request<RoomUsageReport>(`/api/reports/room-usage?${params.toString()}`);
}

export function errorMessage(error: unknown): string {
  return error instanceof Error ? error.message : 'Unknown error';
}

export function toUtcIso(localDateTime: string): string {
  return new Date(localDateTime).toISOString();
}

export function formatDate(iso: string): string {
  return new Date(iso).toLocaleString();
}
