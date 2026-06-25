import Link from "next/link";
import { fetchJobs } from "@/lib/api";
import CloseJobButton from "@/components/CloseJobButton";

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

export async function ListingsTable() {
  const [jobs, stats] = await Promise.all([
    fetchJobs(),
    getApplicationStats(),
  ]);

  if (jobs.length === 0) {
    return (
      <p>No job listings found.</p>
    );
  }

  return (
    <table className="w-full border-collapse">
      <thead>
        <tr className="border-b">
          <th className="text-left p-3">Title</th>
          <th className="text-left p-3">Company</th>
          <th className="text-left p-3">Location</th>
          <th className="text-left p-3">Status</th>
          <th className="text-left p-3">Applications</th>
          <th className="text-left p-3">View</th>
          <th className="text-left p-3">Action</th>
        </tr>
      </thead>

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
                  className="text-blue-600 hover:underline"
                >
                  View
                </Link>
                <td className="p-3">
                <CloseJobButton
                    jobId={job.id}
                    currentStatus={
                    job.isActive
                        ? "Open"
                        : "Closed"
                    }/>
                </td>
              </td>
            </tr>
          );
        })}
      </tbody>
    </table>
  );
}

export function ListingsTableSkeleton() {
  return (
    <div className="animate-pulse">
      {[...Array(5)].map((_, i) => (
        <div
          key={i}
          className="h-12 border-b"
        />
      ))}
    </div>
  );
}