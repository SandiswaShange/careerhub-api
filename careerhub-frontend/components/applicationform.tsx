"use client";

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
  z.infer<typeof applicationSchema>;