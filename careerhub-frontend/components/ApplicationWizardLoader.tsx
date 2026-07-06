"use client";

import dynamic from "next/dynamic";

const ApplicationWizard = dynamic(
  () =>
    import("./ApplicationWizard").then((mod) => ({
      default: mod.ApplicationWizard,
    })),
  {
    ssr: false,
    loading: () => (
      <div className="h-96 animate-pulse rounded-lg border bg-muted" />
    ),
  }
);

type Props = {
  jobId: string;
  jobTitle: string;
};

export function ApplicationWizardLoader(props: Props) {
  return <ApplicationWizard {...props} />;
}   