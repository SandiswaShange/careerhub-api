import { JobListing } from "@/types";//Step 1: Import the type

interface JobListingProps {job: JobListing;}//Step 2: Create props
export function JobCard({job,}: JobListingProps){//Step 3: Create component{
  return (
    <div className="border p-4 rounded">
      <h2 className="text-xl font-semibold">
        {job.title}
      </h2>

      <p>
        Company {job.company} · Location {job.location}
      </p>
      <span className={`px-2 py-1 rounded text-sm ${
        job.employmentType=="Contract" ? "bg-green-100 text-green-700": "bg-red-100 text-red-700"}`}>
       {job.employmentType=="FullTime"? "Available": "Booked"}
    </span>

    </div>
  );
}
//Step 4: Create test room

