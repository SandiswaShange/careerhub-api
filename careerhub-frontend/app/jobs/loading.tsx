export default function Loading() {
  return (
    <main className="p-8">
      <h1 className="text-3xl font-bold mb-6">
        Available Jobs
      </h1>

      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
        {Array.from({ length: 5 }).map(
          (_, index) => (
            <div
              key={index}
              className="
                animate-pulse
                border
                rounded-lg
                p-4
                bg-white
                dark:bg-slate-900
              "
            >
              <div className="h-6 w-3/4 bg-slate-300 dark:bg-slate-700 rounded mb-3" />

              <div className="h-4 w-1/2 bg-slate-300 dark:bg-slate-700 rounded mb-2" />

              <div className="h-4 w-1/3 bg-slate-300 dark:bg-slate-700 rounded mb-4" />

              <div className="h-6 w-24 bg-slate-300 dark:bg-slate-700 rounded" />
            </div>
          )
        )}
      </div>
    </main>
  );
}