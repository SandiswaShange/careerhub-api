import { z } from "zod";

export const applicationWizardSchema = z.object({
  fullName: z
    .string()
    .trim()
    .min(1, "Full name is required"),

  email: z
    .email("Please enter a valid email address"),

  phone: z.string().optional(),

  coverLetter: z.string().optional(),

  linkedInUrl: z
    .string()
    .optional()
    .refine(
      (value) =>
        !value ||
        value.startsWith("https://linkedin.com/") ||
        value.startsWith("https://www.linkedin.com/"),
      {
        message:
          "LinkedIn URL must start with https://linkedin.com/ or https://www.linkedin.com/",
      }
    ),

  heardAboutRole: z
    .string()
    .min(1, "Please tell us how you heard about this role"),
});

export type ApplicationWizardData =
  z.infer<typeof applicationWizardSchema>;