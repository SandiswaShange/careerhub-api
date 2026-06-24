import { NextRequest, NextResponse } from "next/server";

const jobs = [
  {
    id: "550e8400-e29b-41d4-a716-446655440000",
    title: "Junior Software Developer",
    company: "CareerHub",
    location: "Johannesburg",
    status: "Open",
    description:
      "Build and maintain backend services using .NET and PostgreSQL.",
  },
  {
    id: "550e8400-e29b-41d4-a716-446655440001",
    title: "Frontend Developer",
    company: "CareerHub",
    location: "Cape Town",
    status: "Open",
    description:
      "Develop modern React and Next.js user interfaces.",
  },
  {
    id: "550e8400-e29b-41d4-a716-446655440002",
    title: "QA Tester",
    company: "CareerHub",
    location: "London",
    status: "Closed",
    description:
      "Create and execute automated and manual test plans.",
  },
];

export async function GET(
  request: NextRequest,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;

  const job = jobs.find((j) => j.id === id);

  if (!job) {
    return NextResponse.json(
      {
        title: "Job Not Found",
        detail: `No job exists with id '${id}'`,
        status: 404,
      },
      { status: 404 }
    );
  }

  return NextResponse.json(job);
}

export async function POST() {
  return NextResponse.json(
    {
      title: "Method Not Allowed",
      detail: "POST is not supported for this endpoint",
      status: 405,
    },
    { status: 405 }
  );
}