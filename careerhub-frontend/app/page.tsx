"use client";

import { useState, useEffect  } from "react";
import { JobList } from "@/components/JobList";
import { useQuery } from "@tanstack/react-query";
import { fetchJobs } from "@/lib/api";
import { JobListSkeleton } from "@/components/JobCardSkeleton";

export default function Home() {

const [selectedId, setSelectedId] = useState<string | null>(() => {
  if (typeof window === "undefined") {
    return null;
  }

  return sessionStorage.getItem("selectedJobId");
});
const {
  data: jobs,
  isPending,
  isError,
  error,
  refetch,
} = useQuery({
  queryKey: ["jobs"],
  queryFn: fetchJobs,
  //queryFn: () => new Promise(() => {}), for testing skeleton animation
});


useEffect(() => {
  if (selectedId) {
    sessionStorage.setItem("selectedJobId", selectedId);
  } else {
    sessionStorage.removeItem("selectedJobId");
  }
}, [selectedId]);

  const selectedJob =
    jobs?.find((job) => job.id === selectedId) ?? null;

  function handleSelect(id: string) {
    setSelectedId(selectedId === id ? null : id);
  }

  if (isPending) {
  return (
    <main className="p-8">
      <h1 className="text-3xl font-bold mb-6">
        CareerHub Frontend
      </h1>

      <JobListSkeleton />
    </main>
  );
}

if (isError) {
  return (
    <main className="p-8">
      <div className="border rounded p-6 bg-red-50 border-red-200 dark:bg-red-950 dark:border-red-800">
        <h2 className="font-semibold mb-2">
          Failed to load jobs
        </h2>

        <p className="mb-4">
          {error.message}
        </p>

        <button
          onClick={() => refetch()}
          className="px-4 py-2 rounded border"
        >
          Try again
        </button>
      </div>
    </main>
  );
}

  return (
    <main className="p-8">
      <h1 className="text-3xl font-bold mb-6">
        CareerHub Frontend
      </h1>

      {selectedJob && (
        <div className="border rounded p-4 mb-6">
          <h2 className="font-semibold">
            {selectedJob.title}
          </h2>
          <p>{selectedJob.company}</p>
        </div>
      )}

     {jobs && ( <JobList
        jobs={jobs}
        selectedId={selectedId}
        onSelect={handleSelect}
      />
      )}
    </main>
  );
}