"use client";

import { useState, useEffect  } from "react";
import { JobListing } from "@/types";
import { JobList } from "@/components/JobList";
import { jobs } from "@/data/jobs";

export default function Home() {

const [selectedId, setSelectedId] = useState<string | null>(null);

useEffect(() => {
  const storedId = sessionStorage.getItem("selectedJobId");

  if (storedId) {
    setSelectedId(storedId);
  }
}, []);

useEffect(() => {
  if (selectedId) {
    sessionStorage.setItem("selectedJobId", selectedId);
  } else {
    sessionStorage.removeItem("selectedJobId");
  }
}, [selectedId]);

  // EFFECT 2: persist selection whenever it changes
  useEffect(() => {
    if (selectedId) {
      sessionStorage.setItem("selectedJobId", selectedId);
    } else {
      sessionStorage.removeItem("selectedJobId");
    }
  }, [selectedId]);

  const selectedJob =
    jobs.find((job) => job.id === selectedId) ?? null;

  function handleSelect(id: string) {
    setSelectedId(selectedId === id ? null : id);
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

      <JobList
        jobs={jobs}
        selectedId={selectedId}
        onSelect={handleSelect}
      />
    </main>
  );
}