import { JobListing } from "@/types";
import { JobCard } from "./JobCard";

interface JobListProps {
  jobs: JobListing[];
  selectedId: string | null;
  onSelect: (id: string) => void;
}

export function JobList({
  jobs,
  selectedId,
  onSelect,
}: JobListProps) {
  if (jobs.length === 0) {
    return (
      <p>
        No CareerHub vacancies are currently available.
      </p>
    );
  }

  return (
    <>
      <p className="mb-4">
        Showing {jobs.length} jobs
      </p>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {jobs.map((job) => (
          <JobCard
            key={job.id}
            job={job}
            isSelected={selectedId === job.id}
            onSelect={onSelect}
          />
        ))}
      </div>
    </>
  );
}