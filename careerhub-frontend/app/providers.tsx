"use client";

import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { useState } from "react";
import { NuqsAdapter } from 'nuqs/adapters/next/app';

export default function Providers({children,}: {children: React.ReactNode;}) 
{
  const [queryClient] = useState(
    () =>
      new QueryClient({
        defaultOptions: {
          queries: {
            retry: 3,
          },
        },
      })
  );

  return (
    <NuqsAdapter>
      <QueryClientProvider client={queryClient}>
        {children}
        <ReactQueryDevtools initialIsOpen={false} />
      </QueryClientProvider>
    </NuqsAdapter>
  );
}