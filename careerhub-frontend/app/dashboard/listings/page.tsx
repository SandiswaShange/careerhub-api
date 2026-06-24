import Link from "next/link";
import { fetchJobs } from "@/lib/api";

type ApplicationStat = {
  jobId: string;
  applicationCount: number;
};

async function getApplicationStats(): Promise<ApplicationStat[]> {
  const res = await fetch(
    `${process.env.NEXT_PUBLIC_API_URL}/api/applications/stats`,
    {
      cache: "no-store",
    }
  );

  if (!res.ok) {
    throw new Error(
      `Failed to fetch application stats: ${res.status}`
    );
  }

  return res.json();
}

export default async function ListingsPage() {
  const [jobs, stats] = await Promise.all([
  fetchJobs(),
  getApplicationStats(),
]);

  if (jobs.length === 0) {
    return (
      <div>
        <h1 className="text-3xl font-bold">
          Listings
        </h1>

        <p className="mt-4">
          No job listings found.
        </p>
      </div>
    );
  }

  return (
    <>
      <h1 className="text-3xl font-bold mb-2">
        Listings
      </h1>

      <p className="mb-6 text-slate-600 dark:text-slate-400">
        {jobs.length} listings
      </p>

      <table className="w-full border-collapse">
        <thead>
          <tr className="border-b">
            <th className="text-left p-3">
              Title
            </th>

            <th className="text-left p-3">
              Company
            </th>

            <th className="text-left p-3">
              Location
            </th>

            <th className="text-left p-3">
              Status
            </th>

            <th className="text-left p-3">
              Applications
            </th>

            <th className="text-left p-3">
              View
            </th>
          </tr>
        </thead>

        <tbody>
          <tbody>
            {jobs.map((job) => {
              const stat = stats.find(
                (s) => s.jobId === job.id
              );

              const applicationCount =
                stat?.applicationCount ?? 0;

              return (
                <tr
                  key={job.id}
                  className="border-b"
                >
                  <td className="p-3">
                    {job.title}
                  </td>

                  <td className="p-3">
                    {job.company}
                  </td>

                  <td className="p-3">
                    {job.location}
                  </td>

                  <td className="p-3">
                    {job.isActive
                      ? "Open"
                      : "Closed"}
                  </td>

                  <td className="p-3">
                    {applicationCount}
                  </td>

                  <td className="p-3">
                    <Link
                      href={`/jobs/${job.id}`}
                      className="
                        text-blue-600
                        hover:underline
                      "
                    >
                      View
                    </Link>
                  </td>
                </tr>
              );
            })}
          </tbody>
        </tbody>
      </table>
    </>
  );
}