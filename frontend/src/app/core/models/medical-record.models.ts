export interface MedicalRecordDto {
  id: string;
  patientId: string;
  patientName: string;
  doctorId: string;
  doctorName: string;
  appointmentId: string;
  diagnosis: string;
  symptoms: string;
  notes: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateMedicalRecordRequest {
  patientId: string;
  appointmentId: string;
  diagnosis: string;
  symptoms: string;
  notes: string;
}

export interface UpdateMedicalRecordRequest {
  diagnosis: string;
  symptoms: string;
  notes: string;
}
