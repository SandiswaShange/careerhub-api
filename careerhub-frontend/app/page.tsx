"use client";

import { useState, useEffect  } from "react";
import { twMerge } from "tailwind-merge";
import { JobListing } from "@/types";
import { JobList } from "@/components/JobList";

const jobs: JobListing[] = [
  {
    id: "550e8400-e29b-41d4-a716-446655440000",
    title: "Junior Software Developer",
    company: "CareerHub",
    location: "Johannesburg",
    employmentType: "FullTime",
    salaryMin: 25000,
    salaryMax: 40000,
    postedAt: "2026-06-17T14:30:00",
    isActive: true,
    applicantCount: 1,
  },
  {
    id: "550e8400-e29b-41d4-a716-446655440001",
    title: "Frontend Developer",
    company: "CareerHub",
    location: "Cape Town",
    employmentType: "PartTime",
    salaryMin: 30000,
    salaryMax: 50000,
    postedAt: "2026-06-17T14:30:00",
    isActive: true,
    applicantCount: 1,
  },
  {
    id: "550e8400-e29b-41d4-a716-446655440002",
    title: "QA Tester",
    company: "CareerHub",
    location: "London",
    employmentType: "Contract",
    salaryMin: 20000,
    salaryMax: 35000,
    postedAt: "2026-06-17T14:30:00",
    isActive: true,
    applicantCount: 1,
  },
  {
  id: "550e8400-e29b-41d4-a716-446655440003",
  title: "Business Analyst",
  company: "Discovery",
  location: "Pretoria",
  employmentType: "FullTime",
  salaryMin: 35000,
  salaryMax: 55000,
  postedAt: "2026-05-01T10:00:00",
  isActive: false,
  applicantCount: 0
  },
  {
  id: "550e8400-e29b-41d4-a716-446655440004",
  title: "Solution Architect",
  company: "Standard Bank",
  location: "Cape Town",
  employmentType: "FullTime",
  salaryMin: 60000,
  salaryMax: 85000,
  postedAt: "2026-05-01T10:00:00",
  isActive: false,
  applicantCount: 0
  },
  {
  id: "550e8400-e29b-41d4-a716-446655440005",
  title: "DevOps Engineer",
  company: "Transnet",
  location: "Bloemfontein",
  employmentType: "Contract",
  salaryMin: 75000,
  salaryMax: 85000,
  postedAt: "2026-05-01T10:00:00",
  isActive: true,
  applicantCount: 3
  }
];

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