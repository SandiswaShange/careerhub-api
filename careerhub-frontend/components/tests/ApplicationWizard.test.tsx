import { describe, expect, it } from "vitest";
import ApplicationWizard from "../ApplicationWizard";
import { renderWithProviders } from "@/test/utils";
import { vi } from "vitest";
import { screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";

vi.mock("next-auth/react", () => ({
  useSession: () => ({
    data: {
      user: {
        id: "user-1",
        name: "Test User",
        email: "test@example.com",
        role: "candidate",
      },
    },
    status: "authenticated",
  }),
}));

it("moves to step 2 after entering valid details", async () => {
  const user = userEvent.setup();

  renderWithProviders(
    <ApplicationWizard
      jobId="job-1"
      jobTitle="Frontend Developer"
    />
  );

  await user.type(
    screen.getByLabelText(/Full Name/i),
    "John Smith"
  );

  await user.type(
    screen.getByLabelText(/Email/i),
    "john@example.com"
  );

  await user.click(
    screen.getByRole("button", { name: /Next/i })
  );

  await waitFor(() => {
    expect(
      screen.getByText("Your Application")
    ).toBeInTheDocument();
  });
});

describe("ApplicationWizard", () => {
  it("renders step 1 on mount", () => {
    renderWithProviders(
      <ApplicationWizard
        jobId="job-1"
        jobTitle="Frontend Developer"
      />
    );

    expect(
      screen.getByText("Your Details")
    ).toBeInTheDocument();

    expect(
      screen.getByText("Step 1 of 3")
    ).toBeInTheDocument();
  });
});