import { JobListing } from "@/types";//Step 1: Import the type
import { JobStatusBadge } from "./JobStatusBadge";

function formatSalary(min: number, max: number) {
  return `R${min.toLocaleString()} – R${max.toLocaleString()} pm`;
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
return (
    <div
      onClick={() => onSelect(job.id)}
      className={`border rounded p-4 cursor-pointer ${
        isSelected ? "border-blue-500 bg-blue-50" : ""
      }`}
    >
      <h2 className="text-xl font-semibold">
        {job.title}
      </h2>

      <p>
        {job.company} · {job.location}
      </p>

      <span className="inline-block mt-2 px-2 py-1 rounded text-sm bg-gray-100">
        {job.employmentType}
      </span>

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

