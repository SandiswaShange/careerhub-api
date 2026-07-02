import { JobLinkCard } from "@/components/JobLinkCard";
import { ApiJobListing, mapJobListing } from "@/lib/api";

type PagedResponse<T> = {
  data: T[];
};

export default async function JobsPage() {
  let jobs = [];

  try {
    const res = await fetch(
      `${process.env.NEXT_PUBLIC_API_URL}/api/v1/jobs`,
      {
        next: { tags: ["jobs"] },
      }
    );

    if (!res.ok) {
      throw new Error();
    }

    const result =
      (await res.json()) as PagedResponse<ApiJobListing>;

    jobs = result.data.map(mapJobListing);
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

  if (jobs.length === 0) {
    return (
      <main className="p-8">
        <p>No job listings are currently available.</p>
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
          <JobLinkCard key={job.id} job={job} />
        ))}
      </div>
    </main>
  );
}