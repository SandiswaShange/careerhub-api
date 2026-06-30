"use client";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";

import { submitApplication } from "@/lib/api";
import { cn } from "@/lib/utils";

import { z } from "zod";

const phoneRegex = /^\+?[\d\s\-()]{8,15}$/;

export const applicationSchema = z
  .object({
    fullName: z
      .string()
      .min(2, "Full name must be at least 2 characters")
      .max(100, "Full name must not exceed 100 characters"),

    email: z.email("Please enter a valid email address"),

    phone: z
      .union([
        z.string().regex(phoneRegex, "Invalid phone number"),
        z.literal(""),
      ])
      .optional(),

    yearsOfExperience: z.coerce
      .number()
      .int("Must be a whole number")
      .min(0, "Must be at least 0")
      .max(50, "Must be 50 or less"),

    coverLetter: z
      .string()
      .min(
        50,
        "Cover letter must be at least 50 characters — tell us why you're a strong fit"
      )
      .max(2000, "Cover letter must not exceed 2000 characters"),

    linkedInUrl: z
      .union([
        z
          .url("Must be a valid URL")
          .refine(
            (url) => url.includes("linkedin.com"),
            "Must be a LinkedIn URL"
          ),
        z.literal(""),
      ])
      .optional(),

    availableImmediately: z.boolean(),

    noticePeriodWeeks: z.coerce
      .number()
      .int("Must be a whole number")
      .min(0, "Must be at least 0"),
  })
  .refine(
    (data) =>
      data.availableImmediately ||
      data.noticePeriodWeeks > 0,
    {
      message:
        "Notice period must be greater than 0 when not available immediately",
      path: ["noticePeriodWeeks"],
    }
  );

export type ApplicationFormData =
  z.output<typeof applicationSchema>;

type ApplicationFormInput =
  z.input<typeof applicationSchema>;

interface ApplicationFormProps { jobId: string;  jobTitle: string;}

export function ApplicationForm({
  jobId,
  jobTitle,
}: ApplicationFormProps) {
  const queryClient = useQueryClient();

const {
  register,
  handleSubmit,
  reset,
  formState: {
    errors,
    isSubmitting,
  },
} = useForm<
  ApplicationFormInput,
  unknown,
  ApplicationFormData
>({
    resolver: zodResolver(applicationSchema),
    defaultValues: {
      availableImmediately: true,
      noticePeriodWeeks: 0,
      yearsOfExperience: 0,
    },
  });

const mutation = useMutation({
  mutationFn: submitApplication,

  onSuccess: async () => {
    await queryClient.invalidateQueries({
      queryKey: ["jobs"],
    });

    toast.success(
      `Your application for "${jobTitle}" has been submitted successfully.`
    );

    reset();
  },

  onError: (error: Error) => {
    toast.error(error.message);
  },
});

  async function onValid(
    data: ApplicationFormData
  ) {
    await mutation.mutateAsync({
      ...data,
      jobId,
    });
  }

  const isBusy =
    isSubmitting || mutation.isPending;

  return (
    <>
      {/* noValidate prevents browser validation UI so RHF/Zod is the single source of truth */}
      <form
        noValidate
        onSubmit={handleSubmit(onValid)}
        className="space-y-4"
      >
        {/* Full Name */}
        <div>
          <label htmlFor="fullName">
            Full Name
          </label>

          <input
            id="fullName"
            {...register("fullName")}
            aria-invalid={!!errors.fullName}
            className={cn(
              "w-full rounded border p-2",
              "bg-white dark:bg-slate-900",
              errors.fullName
                ? "border-red-500"
                : "border-slate-300 dark:border-slate-700"
            )}
          />

          {errors.fullName && (
            <p className="text-red-500 text-sm">
              {errors.fullName.message}
            </p>
          )}
        </div>

        {/* Email */}
        <div>
          <label htmlFor="email">
            Email
          </label>

          <input
            id="email"
            type="email"
            {...register("email")}
            aria-invalid={!!errors.email}
            className={cn(
              "w-full rounded border p-2",
              errors.email
                ? "border-red-500"
                : "border-slate-300 dark:border-slate-700"
            )}
          />

          {errors.email && (
            <p className="text-red-500 text-sm">
              {errors.email.message}
            </p>
          )}
        </div>

        {/* Phone */}
        <div>
          <label htmlFor="phone">
            Phone
          </label>

          <input
            id="phone"
            {...register("phone")}
            aria-invalid={!!errors.phone}
            className={cn(
              "w-full rounded border p-2",
              errors.phone
                ? "border-red-500"
                : "border-slate-300 dark:border-slate-700"
            )}
          />

          {errors.phone && (
            <p className="text-red-500 text-sm">
              {errors.phone.message}
            </p>
          )}
        </div>

        {/* Years */}
        <div>
          <label htmlFor="yearsOfExperience">
            Years of Experience
          </label>

          <input
            id="yearsOfExperience"
            type="number"
            {...register("yearsOfExperience")}
            aria-invalid={
              !!errors.yearsOfExperience
            }
            className={cn(
              "w-full rounded border p-2",
              errors.yearsOfExperience
                ? "border-red-500"
                : "border-slate-300 dark:border-slate-700"
            )}
          />

          {errors.yearsOfExperience && (
            <p className="text-red-500 text-sm">
              {
                errors.yearsOfExperience
                  .message
              }
            </p>
          )}
        </div>

        {/* Cover Letter */}
        <div>
          <label htmlFor="coverLetter">
            Cover Letter
          </label>

          <textarea
            id="coverLetter"
            rows={6}
            {...register("coverLetter")}
            aria-invalid={
              !!errors.coverLetter
            }
            className={cn(
              "w-full rounded border p-2",
              errors.coverLetter
                ? "border-red-500"
                : "border-slate-300 dark:border-slate-700"
            )}
          />

          {errors.coverLetter && (
            <p className="text-red-500 text-sm">
              {errors.coverLetter.message}
            </p>
          )}
        </div>

        {/* LinkedIn */}
        <div>
          <label htmlFor="linkedInUrl">
            LinkedIn URL
          </label>

          <input
            id="linkedInUrl"
            {...register("linkedInUrl")}
            aria-invalid={
              !!errors.linkedInUrl
            }
            className={cn(
              "w-full rounded border p-2",
              errors.linkedInUrl
                ? "border-red-500"
                : "border-slate-300 dark:border-slate-700"
            )}
          />

          {errors.linkedInUrl && (
            <p className="text-red-500 text-sm">
              {
                errors.linkedInUrl
                  .message
              }
            </p>
          )}
        </div>

        {/* Available Immediately */}
        <div>
          <label>
            <input
              type="checkbox"
              {...register(
                "availableImmediately"
              )}
            />
            Available Immediately
          </label>
        </div>

        {/* Notice Period */}
        <div>
          <label htmlFor="noticePeriodWeeks">
            Notice Period (Weeks)
          </label>

          <input
            id="noticePeriodWeeks"
            type="number"
            {...register(
              "noticePeriodWeeks"
            )}
            aria-invalid={
              !!errors.noticePeriodWeeks
            }
            className={cn(
              "w-full rounded border p-2",
              errors.noticePeriodWeeks
                ? "border-red-500"
                : "border-slate-300 dark:border-slate-700"
            )}
          />

          {errors.noticePeriodWeeks && (
            <p className="text-red-500 text-sm">
              {
                errors.noticePeriodWeeks
                  .message
              }
            </p>
          )}
        </div>

        <button
          type="submit"
          disabled={isBusy}
          className={cn(
            "rounded px-4 py-2",
            isBusy
              ? "bg-slate-400 cursor-not-allowed dark:bg-slate-700"
              : "bg-blue-600 text-white hover:bg-blue-700"
          )}
        >
          {isBusy
            ? "Submitting…"
            : "Submit Application"}
        </button>
      </form>
    </>
  );
}