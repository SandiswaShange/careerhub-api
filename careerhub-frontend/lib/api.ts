import { JobListing } from "@/types";

export async function fetchJobs(): Promise<JobListing[]> {
  const baseUrl = process.env.NEXT_PUBLIC_API_URL;

  const res = await fetch(`${baseUrl}/api/jobs`);

  if (!res.ok) {
    throw new Error(`Failed to fetch jobs: HTTP ${res.status}`);
  }

  return res.json() as Promise<JobListing[]>;
}