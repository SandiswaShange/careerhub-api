import { NextRequest, NextResponse } from "next/server";
import { jobs } from "@/data/jobs";

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