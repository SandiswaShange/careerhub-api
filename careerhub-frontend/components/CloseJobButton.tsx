"use client";

import { useActionState } from "react";
import {
  closeJobListing,
  type CloseJobState,
} from "@/app/actions/closeJob";

type Props = {
  jobId: string;
  currentStatus: string;
};

export default function CloseJobButton({
  jobId,
  currentStatus,
}: Props) {
  const [state, formAction, isPending] =
    useActionState<CloseJobState, FormData>(
      closeJobListing,
      null
    );

  if (currentStatus === "Closed") {
    return null;
  }

  if (state?.status === "success") {
    return (
      <span>
        Closed: {state.jobTitle}
      </span>
    );
  }

  return (
    <>
      <form action={formAction}>
        <input
          type="hidden"
          name="jobId"
          value={jobId}
        />

        <button
          type="submit"
          disabled={isPending}
          className="border rounded px-2 py-1"
        >
          {isPending
            ? "Closing…"
            : "Close"}
        </button>
      </form>

      {state?.status === "error" && (
        <p className="text-red-600 text-sm mt-1">
          {state.message}
        </p>
      )}
    </>
  );
}