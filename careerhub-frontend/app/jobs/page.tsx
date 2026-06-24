import { JobLinkCard } from "@/components/JobLinkCard";
import { JobListing } from "@/types";
import { ApiJobListing, mapJobListing,} from "@/lib/api";

type PagedResponse<T> = {
  data: T[];
};  

export default async function JobsPage() {
  const res = await fetch(
    `${process.env.NEXT_PUBLIC_API_URL}/api/v1/jobs`,
    {
      cache: "no-store",
    }
  );

  if (!res.ok) {
    throw new Error(
      `Failed to load jobs: ${res.status}`
    );
  }

const result =
  await res.json() as PagedResponse<ApiJobListing>;

const jobs =
  result.data.map(mapJobListing);

  if (jobs.length === 0) {
    return (
      <main className="p-8">
        <p>
          No job listings are currently
          available.
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
        {jobs.map((job) => (
          <JobLinkCard
            key={job.id}
            job={job}
          />
        ))}
      </div>
    </main>
  );
}