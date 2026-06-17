export interface JobListing {
  id: string;
  title: string;
  company: number;
  location: number;
  employmentType: EmploymentType;
  salaryMin: number;
  salaryMax: number;
  postedAt: string;
  isActive: boolean;
  applicantCount: boolean;
}

export type EmploymentType =
  | "FullTime"
  | "PartTime"
  | "Contract"
  | "Internship";