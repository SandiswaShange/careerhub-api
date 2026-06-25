import { NextRequest, NextResponse } from "next/server";

export async function POST(request: NextRequest) {
  const body = await request.json();

  if (!body.jobId || !body.email) {
    return NextResponse.json(
      {
        title: "Validation Error",
        detail: "jobId and email are required",
        status: 400,
      },
      { status: 400 }
    );
  }

  await new Promise<void>((resolve) =>
    setTimeout(resolve, 800)
  );

  return NextResponse.json(
    {
      id: crypto.randomUUID(),
      jobId: body.jobId,
      email: body.email,
      submittedAt: new Date().toISOString(),
    },
    { status: 201 }
  );
}

export async function GET() {
  return NextResponse.json(
    {
      title: "Method Not Allowed",
      detail: "GET is not supported",
      status: 405,
    },
    { status: 405 }
  );
}