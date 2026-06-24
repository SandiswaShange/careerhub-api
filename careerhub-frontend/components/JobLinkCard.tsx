import Link from "next/link";
import { JobListing } from "@/types";
import { JobStatusBadge } from "./JobStatusBadge";

interface JobLinkCardProps {
  job: JobListing;
}

export function JobLinkCard({
  job,
}: JobLinkCardProps) {
  return (
    <Link
      href={`/jobs/${job.id}`}
      className="
        block
        border
        rounded-lg
        p-4
        transition
        hover:shadow-md
        bg-white
        border-slate-200
        dark:bg-slate-900
        dark:border-slate-800
      "
    >
      <h2 className="text-xl font-semibold">
        {job.title}
      </h2>

      <p className="mt-1">
        {job.company}
      </p>

      <p className="text-sm text-slate-500">
        {job.location}
      </p>

      <div className="mt-3">
        <JobStatusBadge
          employmentType={job.employmentType}
        />
      </div>
    </Link>
  );
}