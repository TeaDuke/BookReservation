export interface Room {
  id: number;
  name: string;
  capacity: number;
  floor: number;
  isActive: boolean;
  equipments: string[];
}

export interface Equipment {
  id: number;
  name: string;
  rooms: string[];
}

export interface User {
  id: number;
  firstName: string;
  lastName: string;
  activeReservations: number;
}

export type ReservationStatus = 'Booked' | 'Completed' | 'Cancelled';

export interface Reservation {
  id: number;
  room: Room;
  organizer: User;
  title: string;
  startDate: string;
  endDate: string;
  endedAt: string | null;
  attendees: number;
  status: ReservationStatus;
}

export interface CreateReservationRequest {
  roomId: number;
  organizerId: number;
  title: string;
  startDate: string;
  endDate: string;
  attendees: number;
}

export interface RoomUsage {
  roomId: number;
  roomName: string;
  floor: number;
  totalReservations: number;
  completedCount: number;
  cancelledCount: number;
  endedEarlyCount: number;
  bookedHours: number;
  usedHours: number;
  unusedHours: number;
  utilizationPercent: number;
  isUnderused: boolean;
}

export interface RoomUsageReport {
  from: string;
  to: string;
  rooms: RoomUsage[];
}
