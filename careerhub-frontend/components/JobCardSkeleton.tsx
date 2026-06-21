import React from "react";

export function JobCardSkeleton() {
  return (
    <div className="border rounded p-4">
      <div className="animate-pulse">
        {/* Title */}
        <div className="h-7 w-3/4 rounded bg-slate-200 dark:bg-slate-700 mb-4" />

        {/* Company + location */}
        <div className="h-4 w-2/3 rounded bg-slate-200 dark:bg-slate-700 mb-4" />

        {/* Employment type badge */}
        <div className="h-8 w-24 rounded bg-slate-200 dark:bg-slate-700 mb-4" />

        {/* Salary */}
        <div className="h-4 w-1/2 rounded bg-slate-200 dark:bg-slate-700 mb-3" />

        {/* Posted date */}
        <div className="h-4 w-1/3 rounded bg-slate-200 dark:bg-slate-700 mb-3" />

        {/* Status line */}
        <div className="h-4 w-20 rounded bg-slate-200 dark:bg-slate-700 mb-3" />

        {/* Applicant count */}
        <div className="h-4 w-24 rounded bg-slate-200 dark:bg-slate-700" />
      </div>
    </div>
  );
}

export function JobListSkeleton() {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
      {Array.from({ length: 6 }).map((_, index) => (
        <JobCardSkeleton key={index} />
      ))}
    </div>
  );
}