import { JobListing } from "@/types";
import Image from "next/image";

const rooms: JobListing[] = [
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
    applicantCount: 1
  },
  {
    id: "550e8400-e29b-41d4-a716-446655440000",
    title: "Frontend Developer",
    company: "CareerHub",
    location: "Cape Town",
    employmentType: "PartTime",
    salaryMin: 30000,
    salaryMax: 50000,
    postedAt: "2026-06-17T14:30:00",
    isActive: true,
    applicantCount: 1
  },
  {
    id: "550e8400-e29b-41d4-a716-446655440000",
    title: "QA Tester",
    company: "CareerHub",
    location: "London",
    employmentType: "Contract",
    salaryMin: 20000,
    salaryMax: 35000,
    postedAt: "2026-06-17T14:30:00",
    isActive: true,
    applicantCount: 1
  },
];

export default function Home() {
  return (
    <main className="p-8">
      <h1 className="text-3xl font-bold">
        CareeHub API frontend
      </h1>
    </main>
  );
}
