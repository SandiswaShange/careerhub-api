"use client";

import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

import {
  applicationWizardSchema,
  type ApplicationWizardData,
} from "@/lib/applicationWizardSchema";

type Props = {
  jobId: string;
  jobTitle: string;
};

export default function ApplicationWizard({
  jobId,
  jobTitle,
}: Props) {
  const [step, setStep] = useState(1);

  const form = useForm<ApplicationWizardData>({
    resolver: zodResolver(applicationWizardSchema),
    defaultValues: {
      fullName: "",
      email: "",
      phone: "",
      coverLetter: "",
      linkedInUrl: "",
      heardAboutRole: "",
    },
  });

  const storageKey = `careerhub-application-${jobId}`;

  const {
  register,
  handleSubmit,
  trigger,
  formState: { errors },
} = form;

const watchedValues = form.watch();

useEffect(() => {
  localStorage.setItem(
    storageKey,
    JSON.stringify(watchedValues)
  );
}, [watchedValues, storageKey]);

useEffect(() => {
  const savedDraft = localStorage.getItem(storageKey);

  if (!savedDraft) return;

  try {
    const values = JSON.parse(savedDraft);

    form.reset(values);
  } catch {
    localStorage.removeItem(storageKey);
  }
}, [form, storageKey]);

async function nextStep() {
  if (step === 1) {
    const valid = await trigger([
      "fullName",
      "email",
      "phone",
    ]);

    if (!valid) return;
  }

  if (step === 2) {
    const valid = await trigger([
      "coverLetter",
      "linkedInUrl",
      "heardAboutRole",
    ]);

    if (!valid) return;
  }

  setStep((current) => current + 1);
}

  return (
    <div className="max-w-2xl mx-auto space-y-6">

      <div>
        <h2 className="text-2xl font-bold">
          Apply for {jobTitle}
        </h2>

        <p className="text-muted-foreground">
          Step {step} of 3
        </p>
      </div>

      <div className="rounded-lg border p-6">

    {step === 1 && (
    <div className="space-y-4">

        <h3 className="text-lg font-semibold">
        Your Details
        </h3>

        <div>
        <label htmlFor="fullName">
            Full Name
        </label>

        <input
            id="fullName"
            {...register("fullName")}
            className="w-full rounded border p-2"
        />

        {errors.fullName && (
            <p className="text-sm text-red-600">
            {errors.fullName.message}
            </p>
        )}
        </div>

        <div>
        <label htmlFor="email">
            Email
        </label>

        <input
            id="email"
            type="email"
            {...register("email")}
            className="w-full rounded border p-2"
        />

        {errors.email && (
            <p className="text-sm text-red-600">
            {errors.email.message}
            </p>
        )}
        </div>

        <div>
        <label htmlFor="phone">
            Phone (optional)
        </label>

        <input
            id="phone"
            {...register("phone")}
            className="w-full rounded border p-2"
        />

        {errors.phone && (
            <p className="text-sm text-red-600">
            {errors.phone.message}
            </p>
        )}
        </div>

    </div>
    )}

    {step === 2 && (
        <div className="space-y-4">

            <h3 className="text-lg font-semibold">
            Your Application
            </h3>

            {/* Cover Letter */}
            <div>
            <label htmlFor="coverLetter">
                Cover Letter (optional)
            </label>

            <textarea
                id="coverLetter"
                rows={6}
                {...register("coverLetter")}
                className="w-full rounded border p-2"
            />

            {errors.coverLetter && (
                <p className="text-sm text-red-600">
                {errors.coverLetter.message}
                </p>
            )}
            </div>

            {/* LinkedIn */}
            <div>
            <label htmlFor="linkedInUrl">
                LinkedIn Profile (optional)
            </label>

            <input
                id="linkedInUrl"
                {...register("linkedInUrl")}
                className="w-full rounded border p-2"
            />

            {errors.linkedInUrl && (
                <p className="text-sm text-red-600">
                {errors.linkedInUrl.message}
                </p>
            )}
            </div>

            {/* Heard About Role */}
            <div>
            <label htmlFor="heardAboutRole">
                How did you hear about this role?
            </label>

            <select
                id="heardAboutRole"
                {...register("heardAboutRole")}
                className="w-full rounded border p-2"
            >
                <option value="">
                Select...
                </option>

                <option value="LinkedIn">
                LinkedIn
                </option>

                <option value="Company Website">
                Company Website
                </option>

                <option value="Friend">
                Friend
                </option>

                <option value="Job Board">
                Job Board
                </option>

                <option value="Other">
                Other
                </option>
            </select>

            {errors.heardAboutRole && (
                <p className="text-sm text-red-600">
                {errors.heardAboutRole.message}
                </p>
            )}
            </div>

        </div>
        )}

        {step === 3 && (
          <div>
            <h3 className="font-semibold text-lg">
              Review & Submit
            </h3>

            <p className="text-sm text-muted-foreground">
              Step 3 content goes here.
            </p>
          </div>
        )}

      </div>

      <div className="flex justify-between">

        <button
          type="button"
          disabled={step === 1}
          onClick={() => setStep(step - 1)}
          className="rounded border px-4 py-2 disabled:opacity-50"
        >
          Back
        </button>

        <button
          type="button"
          disabled={step === 3}
          onClick={nextStep}
          className="rounded bg-blue-600 px-4 py-2 text-white disabled:opacity-50"
        >
          Next
        </button>

      </div>

    </div>
  );
}