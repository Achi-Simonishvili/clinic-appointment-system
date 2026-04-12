export interface AppointmentDto {
  id: string;
  patientId: string;
  patientName: string;
  doctorId: string;
  doctorName: string;
  appointmentDate: string;
  startTime: string;
  endTime: string;
  status: string;
  notes: string;
}

export interface BookAppointmentRequest {
  doctorId: string;
  appointmentDate: string;
  startTime: string;
}
