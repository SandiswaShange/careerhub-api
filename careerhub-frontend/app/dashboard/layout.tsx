import Link from "next/link";

export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div className="flex min-h-screen">
      <aside
        className="
          w-64
          border-r
          p-6
          bg-slate-50
          dark:bg-slate-900
          dark:border-slate-800
        "
      >
        <h2 className="text-xl font-semibold mb-6">
          Employer Dashboard
        </h2>

        <nav className="space-y-3">
          <div>
            <Link
              href="/dashboard/listings"
              className="hover:underline"
            >
              All Listings
            </Link>
          </div>

          <div>
            <Link
              href="/jobs"
              className="hover:underline"
            >
              View as Candidate
            </Link>
          </div>
        </nav>
      </aside>

      <section className="flex-1 p-8">
        {children}
      </section>
    </div>
  );
}