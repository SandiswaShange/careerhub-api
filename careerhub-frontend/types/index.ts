/*export interface JobListing {
  id: string;
  title: string;
  company: string;
  location: string;
  type: number;
  applicationCount: number;
}*/
export interface JobListing {
  id: string;
  title: string;
  company: string;
  location: string;
  employmentType: EmploymentType;
  salaryMin: number;
  salaryMax: number;
  postedAt: string;
  isActive: boolean;
  applicantCount: number;
}

export type EmploymentType =
  | "FullTime"
  | "PartTime"
  | "Contract"
  | "Internship";
/*===================================================Assignment 1.4==================================================================*/
export interface ApplicationRequest {
  jobId: string;
  fullName: string;
  email: string;
  phone?: string;
  yearsOfExperience: number;
  coverLetter: string;
  linkedInUrl?: string;
  availableImmediately: boolean;
  noticePeriodWeeks: number;
}

export interface ApplicationResponse {
  id: string;
  jobId: string;
  email: string;
  submittedAt: string;
}
export interface JobDetail {
  id: string;
  title: string;
  description: string;
  company: string;
  location: string;
  type: number;
  postedAt: string;
  minSalary: number | null;
  maxSalary: number | null;
  isActive: boolean;
  applications: ApplicationResponse[];
}