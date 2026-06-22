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

export async function fetchJobs(): Promise<JobListing[]> {
  const baseUrl = process.env.NEXT_PUBLIC_API_URL;

  const res = await fetch(`${baseUrl}/api/v1/jobs`);

  if (!res.ok) {
    throw new Error(`Failed to fetch jobs: HTTP ${res.status}`);
  }

  const result =
    (await res.json()) as PagedResponse<JobListing>;

  return result.data;
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