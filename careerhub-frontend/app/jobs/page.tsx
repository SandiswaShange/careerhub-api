import { JobLinkCard } from "@/components/JobLinkCard";
import { ApiJobListing, mapJobListing } from "@/lib/api";
import { fetchJobs } from "@/lib/api";

type PagedResponse<T> = {
  data: T[];
};

export default async function JobsPage() {
  let jobs = [];

  try {
 jobs = await fetchJobs();
  } catch {
    return (
      <main className="p-8">
        <h1 className="text-3xl font-bold mb-6">
          Available Jobs
        </h1>

        <div className="rounded border border-yellow-300 bg-yellow-50 p-4">
          Unable to load jobs. Is the backend running?
        </div>
      </main>
    );
  }

  const activeJobs = jobs.filter((job) => job.isActive);

if (jobs.length === 0) {
  return (
    <main className="p-8 flex flex-col items-center justify-center">
      <h1 className="text-2xl font-bold">
        No jobs available
      </h1>

      <p className="mt-2 text-muted-foreground">
        There are currently no job listings.
        Please check back later.
      </p>
    </main>
  );
}

  if (activeJobs.length === 0) {
    return (
      <main className="p-8 flex flex-col items-center justify-center">
        <h1 className="text-2xl font-bold">
          No open positions
        </h1>

        <p className="mt-2 text-muted-foreground">
          All current job listings have closed.
        </p>
      </main>
    );
  }

  return (
    <main className="p-8">
      <h1 className="text-3xl font-bold mb-6">
        Available Jobs
      </h1>

      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
        {activeJobs.map((job) => (
          <JobLinkCard key={job.id} job={job} />
        ))}
      </div>
    </main>
  );
}