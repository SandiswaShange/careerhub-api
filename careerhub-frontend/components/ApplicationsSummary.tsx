import React from "react";

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

export async function ApplicationsSummary() {
  const stats = await getApplicationStats();

  const totalApplications = stats.reduce(
    (sum, stat) => sum + stat.applicationCount,
    0
  );

  return (
    <div className="border rounded p-6 mb-6">
      <h2 className="text-lg font-semibold">
        Total Applications
      </h2>

      <p className="text-4xl font-bold mt-2">
        {totalApplications}
      </p>
    </div>
  );
}

export function ApplicationsSummarySkeleton() {
  return (
    <div className="border rounded p-6 mb-6 animate-pulse">
      <div className="h-5 w-40 bg-slate-200 rounded" />
      <div className="h-10 w-24 bg-slate-200 rounded mt-3" />
    </div>
  );
}