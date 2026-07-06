import Link from "next/link";
import { notFound } from "next/navigation";
//import { ApplicationForm, ApplicationFormData } from "@/components/applicationform";
import { JobDetail } from "@/types";
import { ApplicationWizardLoader } from "@/components/ApplicationWizardLoader";
import { fetchJob } from "@/lib/api";

interface PageProps {
  params: Promise<{
    id: string;
  }>;
}

export default async function JobDetailPage({
  params,
}: PageProps) {
  const { id } = await params;

const job = await fetchJob(id);

const isClosed = !job.isActive;

  return (
    <main className="p-8">
      <Link
        href="/jobs"
        className="text-blue-600 hover:underline"
      >
        ← Back to jobs
      </Link>

      <div className="mt-6">
        <h1 className="text-3xl font-bold">
          {job.title}
        </h1>

        <p className="mt-2">
          {job.company}
        </p>

        <p className="text-slate-500">
          {job.location}
        </p>

        <div className="mt-6">
          <h2 className="font-semibold">
            Description
          </h2>

          <p className="mt-2 whitespace-pre-wrap">
            {job.description}
          </p>
        </div>

        <div className="mt-6">
          <h2 className="font-semibold">
            Status
          </h2>

          <p>
            {isClosed ? "Closed" : "Open"}
          </p>
        </div>
      </div>

      <div className="mt-8">
        {!isClosed ? (
          <ApplicationWizardLoader
            jobId={job.id}
            jobTitle={job.title}
          />
        ) : (
          <div className="border rounded p-4">
            Applications for this job
            are closed.
          </div>
        )}
      </div>
    </main>
  );
}