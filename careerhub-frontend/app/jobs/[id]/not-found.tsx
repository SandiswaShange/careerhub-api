import Link from "next/link";

export default function NotFound() {
  return (
    <main className="p-8">
      <h1 className="text-3xl font-bold mb-4">
        Job Not Found
      </h1>

      <p className="mb-6">
        The job listing you requested
        does not exist or has been removed.
      </p>

      <Link
        href="/jobs"
        className="text-blue-600 hover:underline"
      >
        Return to job listings
      </Link>
    </main>
  );
}