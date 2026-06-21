import { JobListing } from "@/types";//Step 1: Import the type
import { JobStatusBadge } from "./JobStatusBadge";
import { cn } from "@/lib/utils";

function formatSalary(min: number, max: number) {
  return `R${min} - R${max} pm`;
}

function getRelativeDate(postedAt: string) {
  const posted = new Date(postedAt);
  const today = new Date();

  const diffDays = Math.floor(
    (today.getTime() - posted.getTime()) / (1000 * 60 * 60 * 24)
  );

  if (diffDays === 0) return "Today";
  if (diffDays === 1) return "1 day ago";
  if (diffDays < 30) return `${diffDays} days ago`;

  const months = Math.floor(diffDays / 30);
  return `${months} month${months > 1 ? "s" : ""} ago`;
}

interface JobListingProps {job: JobListing; isSelected: boolean; onSelect: (id: string) => void;}//Step 2: Create props
export function JobCard({job,isSelected,onSelect,}: JobListingProps){//Step 3: Create component{
const cardClass = cn(
    "border rounded p-4 cursor-pointer transition-all",
    "bg-white text-slate-900 border-slate-200",
    "dark:bg-slate-900 dark:text-slate-100 dark:border-slate-800",

    // selected state (required: visible in BOTH themes)
    isSelected &&
      "ring-2 ring-blue-500 border-blue-400 dark:ring-blue-400",

    // expired state (card-level requirement)
    !job.isActive &&
      "opacity-60 grayscale border-red-300 dark:border-red-800"
  );

  return (
    <div
      onClick={() => onSelect(job.id)}
      className={cardClass}
    >
      <h2 className="text-xl font-semibold">
        {job.title}
      </h2>

      <p>
        {job.company} · {job.location}
      </p>

      <div className="mt-2">
        <JobStatusBadge employmentType={job.employmentType} />
      </div>

      <p className="mt-2">
        {formatSalary(job.salaryMin, job.salaryMax)}
      </p>

      <p className="text-sm text-gray-500">
        Posted {getRelativeDate(job.postedAt)}
      </p>

      {!job.isActive && (
        <p className="text-red-600 font-medium">
          Closed
        </p>
      )}

      {job.applicantCount > 0 && (
        <p>{job.applicantCount} applicants</p>
      )}
    </div>
  );
}
//Step 4: Create test room

