"use server";

import { revalidateTag } from "next/cache";

export type CloseJobState =
  | {
      status: "success";
      jobTitle: string;
    }
  | {
      status: "error";
      message: string;
    }
  | null;

export async function closeJobListing(
  prevState: CloseJobState,
  formData: FormData
): Promise<CloseJobState> {
  const jobId = formData.get("jobId")?.toString();

  if (!jobId?.trim()) {
    return {
      status: "error",
      message: "Job ID is required",
    };
  }

  const response = await fetch(
    `${process.env.NEXT_PUBLIC_API_URL}/api/jobs/${jobId}`,
    {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        status: "Closed",
      }),
    }
  );

  if (!response.ok) {
    let message = "Failed to close job";

    try {
      const problem = await response.json();

      if (problem?.detail) {
        message = problem.detail;
      }
    } catch {
      // ignore parse errors
    }

    return {
      status: "error",
      message,
    };
  }

  const updatedJob = await response.json();

  revalidateTag("jobs", "max");

  return {
    status: "success",
    jobTitle: updatedJob.title,
  };
}