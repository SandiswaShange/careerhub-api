import Link from "next/link";

export default function HomePage() {
  return (
    <div className="max-w-4xl mx-auto px-8 py-16">
      <h1 className="text-5xl font-bold mb-6">
        CareerHub
      </h1>

      <p className="text-lg mb-8 text-slate-600 dark:text-slate-400">
        CareerHub connects employers with talented candidates.
        Browse open opportunities as a candidate or manage
        listings through the employer dashboard.
      </p>

      <div className="flex gap-4">
        <Link
          href="/jobs"
          className="
            rounded
            px-6
            py-3
            bg-blue-600
            text-white
            hover:bg-blue-700
          "
        >
          Browse Jobs
        </Link>

        <Link
          href="/dashboard/listings"
          className="
            rounded
            px-6
            py-3
            border
            border-slate-300
            hover:bg-slate-100
            dark:border-slate-700
            dark:hover:bg-slate-800
          "
        >
          Employer Dashboard
        </Link>
      </div>
    </div>
  );
}