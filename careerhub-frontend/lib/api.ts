import { JobListing } from "@/types";
import {ApplicationRequest,ApplicationResponse,} from "@/types";

type PagedResponse<T> = {
  data: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
};
export type ApiJobListing = {
  id: string;
  title: string;
  company: string;
  location: string;
  type: number;
  minSalary: number;
  maxSalary: number;
  postedAt: string;
  isActive: boolean;
  applicationCount: number;
};
export function mapJobListing(
  job: ApiJobListing
): JobListing {
  return {
    id: job.id,
    title: job.title,
    company: job.company,
    location: job.location,

    employmentType:
      job.type === 0
        ? "FullTime"
        : job.type === 1
        ? "PartTime"
        : job.type === 2
        ? "Contract"
        : "Internship",

    salaryMin: job.minSalary,
    salaryMax: job.maxSalary,

    postedAt: job.postedAt,
    isActive: job.isActive,

    applicantCount: job.applicationCount,
  };
}   

export async function fetchJobs(): Promise<JobListing[]> {
  const baseUrl = process.env.NEXT_PUBLIC_API_URL;

  const res = await fetch(`${baseUrl}/api/v1/jobs`);

  if (!res.ok) {
    throw new Error(`Failed to fetch jobs: HTTP ${res.status}`);
  }

  const result = await res.json()as PagedResponse<ApiJobListing>;

  return result.data.map(mapJobListing);
}

/*============================================Assignment 1.4=====================================================================*/
export async function submitApplication(
  application: ApplicationRequest
): Promise<ApplicationResponse> {
  const baseUrl = process.env.NEXT_PUBLIC_API_URL;

  const res = await fetch(
    `${baseUrl}/api/applications`,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(application),
    }
  );

  if (!res.ok) {
    const problem = await res.json();

    throw new Error(
      problem.detail ?? problem.title
    );
  }

  return res.json() as Promise<ApplicationResponse>;
}