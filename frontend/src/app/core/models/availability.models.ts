export interface DoctorAvailabilityDto {
  id: string;
  doctorId: string;
  dayOfWeek: string;
  startTime: string;
  endTime: string;
  slotDurationMinutes: number;
  isAvailable: boolean;
}

export interface SetAvailabilityRequest {
  dayOfWeek: string;
  startTime: string;
  endTime: string;
  slotDurationMinutes: number;
}
