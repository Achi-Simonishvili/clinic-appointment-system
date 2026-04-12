export interface DoctorDto {
  id: string;
  userId: string;
  fullName: string;
  email: string;
  licenseNumber: string;
  bio: string;
  specializationId: string;
  specializationName: string;
  departmentId: string;
  departmentName: string;
  phoneNumber: string;
  isActive: boolean;
}

export interface DoctorFilterRequest {
  search?: string;
  specialization?: string;
  department?: string;
  pageNumber?: number;
  pageSize?: number;
}
