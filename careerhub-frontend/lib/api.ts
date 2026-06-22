import { JobListing } from "@/types";

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