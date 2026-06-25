import { Suspense } from "react";

import {
  ApplicationsSummary,
  ApplicationsSummarySkeleton,
} from "@/components/ApplicationsSummary";

import {
  ListingsTable,
  ListingsTableSkeleton,
} from "@/components/ListingsTable";

export default async function ListingsPage() {
  return (
    <>
      <h1 className="text-3xl font-bold mb-2">
        Listings
      </h1>

      <Suspense
        fallback={
          <ApplicationsSummarySkeleton />
        }
      >
        <ApplicationsSummary />
      </Suspense>

      <Suspense
        fallback={
          <ListingsTableSkeleton />
        }
      >
        <ListingsTable />
      </Suspense>
    </>
  );
}