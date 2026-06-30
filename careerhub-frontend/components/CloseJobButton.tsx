"use client";

import { useActionState, useEffect } from "react";
import { closeJobListing, type CloseJobState,} from "@/app/actions/closeJob";
import { toast } from "sonner";

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

    useEffect(() => {
  if (!state) return;

  if (state.status === "success") {
    toast.success(`"${state.jobTitle}" has been closed.`);
  }

  if (state.status === "error") {
    toast.error(state.message);
  }}, [state]);

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
    </>
  );
}