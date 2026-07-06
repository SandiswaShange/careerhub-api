import Link from "next/link";
import { JobListing } from "@/types";
import { JobStatusBadge } from "./JobStatusBadge";
import Image from "next/image";

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
    <div className="flex items-center gap-3">
      <Image
        src="/company-logo.svg"
        alt={`${job.company} logo`}
        width={40}
        height={40}
      />

      <div>
        <h2 className="text-xl font-semibold">
          {job.title}
        </h2>

        <p className="mt-1">
          {job.company}
        </p>
      </div>
    </div>

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