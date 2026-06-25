import { NextResponse } from "next/server";
import { jobs } from "@/data/jobs";

export async function GET() {
  const stats = jobs.map((job, index) => ({
    jobId: job.id,
    applicationCount: (index + 1) * 5,
  }));

  return NextResponse.json(stats);
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