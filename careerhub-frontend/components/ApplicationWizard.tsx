"use client";
import { useEffect, useMemo, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useSession } from "next-auth/react";
import { toast } from "sonner";
import { submitApplication } from "@/lib/api";
import { applicationWizardSchema, type ApplicationWizardData,} from "@/lib/applicationWizardSchema";
import { AlertDialog, AlertDialogAction, AlertDialogCancel, AlertDialogContent, AlertDialogDescription, AlertDialogFooter, AlertDialogHeader, AlertDialogTitle, AlertDialogTrigger, } from "@/components/ui/alert-dialog";

type Props = {
  jobId: string;
  jobTitle: string;
};

type UserWithRole = {
  role?: string;
};

function isCandidateSession(session: unknown) {
  const role = (session as { user?: UserWithRole } | null)?.user?.role;
  return role === "CANDIDATE" || role === "candidate";
}

function displayValue(value?: string | null) {
  return value && value.trim() ? value : "Not provided";
}

export function ApplicationWizard({ jobId, jobTitle }: Props) {
  const [step, setStep] = useState(1);
  const [signInMessage, setSignInMessage] = useState<string | null>(null);
  const [draftReady, setDraftReady] = useState(false);
  const [draftRestored, setDraftRestored] = useState(false);
  const [hasDraft, setHasDraft] = useState(false);

  const storageKey = useMemo(() => `careerhub-application-${jobId}`, [jobId]);

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

  const { data: session, status } = useSession();
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: submitApplication,
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["jobs"],
      });

      localStorage.removeItem(storageKey);
      toast.success("Application submitted successfully.");
      form.reset();
      setStep(1);
    },
    onError: (error: Error) => {
      toast.error(error.message);
    },
  });

  const {
    register,
    handleSubmit,
    trigger,
    formState: { errors },
  } = form;

  const watchedValues = form.watch();

  useEffect(() => {
    const savedDraft = localStorage.getItem(storageKey);

    if (savedDraft) {
  try {
    form.reset(JSON.parse(savedDraft));
    setDraftRestored(true);
    setHasDraft(true);
  } catch {
    localStorage.removeItem(storageKey);
  }
}

    setDraftReady(true);
  }, [form, storageKey]);

  useEffect(() => {
    if (!draftReady) return;
    localStorage.setItem(storageKey, JSON.stringify(watchedValues));
    setHasDraft(true);
  }, [draftReady, storageKey, watchedValues]);

  async function nextStep() {
    setSignInMessage(null);

    if (step === 1) {
      const valid = await trigger(["fullName", "email"]);
      if (!valid) return;

      if (status !== "authenticated" || !isCandidateSession(session)) {
        setSignInMessage("Please sign in as a candidate to continue.");
        return;
      }
    }

    if (step === 2) {
      const valid = await trigger([
        "phone",
        "coverLetter",
        "linkedInUrl",
        "heardAboutRole",
      ]);

      if (!valid) return;
    }

    setStep((current) => current + 1);
  }

  function discardDraft() {
  localStorage.removeItem(storageKey);

  form.reset();

  setStep(1);

  setDraftRestored(false);

  setHasDraft(false);
}

  async function onSubmit(data: ApplicationWizardData) {
    await mutation.mutateAsync({jobId,...data,});
  }

  if (status === "authenticated" && session && !isCandidateSession(session)) {
    return (
      <div className="max-w-2xl mx-auto rounded-lg border p-6">
        <h2 className="text-2xl font-bold">Apply for {jobTitle}</h2>
        <p className="mt-4 text-red-600">Employers cannot apply for jobs.</p>
      </div>
    );
  }

  return (
    <form
      onSubmit={handleSubmit(onSubmit)}
      className="mx-auto max-w-2xl space-y-6"
    >
      <div>
        <h2 className="text-2xl font-bold">Apply for {jobTitle}</h2>
        <p className="text-muted-foreground">Step {step} of 3</p>
      </div>

      {signInMessage && (
        <p className="rounded border border-amber-300 bg-amber-50 p-3 text-sm text-amber-800">
          {signInMessage}
        </p>
      )}

      {draftRestored && (
  <div className="rounded border border-green-300 bg-green-50 p-3 flex justify-between items-center">
    <span>
      You have a saved draft for this application. Restored automatically.
    </span>

    <button
      type="button"
      onClick={() => setDraftRestored(false)}
      className="text-sm underline"
    >
      Dismiss
    </button>
  </div>
)}

      <div className="rounded-lg border p-6">
        {step === 1 && (
          <div className="space-y-4">
            <h3 className="text-lg font-semibold">Your Details</h3>

            <div>
              <label htmlFor="fullName">Full Name</label>
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
              <label htmlFor="email">Email</label>
              <input
                id="email"
                type="email"
                {...register("email")}
                className="w-full rounded border p-2"
              />
              {errors.email && (
                <p className="text-sm text-red-600">{errors.email.message}</p>
              )}
            </div>

            <div>
              <label htmlFor="phone">Phone (optional)</label>
              <input
                id="phone"
                {...register("phone")}
                className="w-full rounded border p-2"
              />
              {errors.phone && (
                <p className="text-sm text-red-600">{errors.phone.message}</p>
              )}
            </div>
          </div>
        )}

        {step === 2 && (
          <div className="space-y-4">
            <h3 className="text-lg font-semibold">Your Application</h3>

            <div>
              <label htmlFor="coverLetter">Cover Letter (optional)</label>
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

            <div>
              <label htmlFor="linkedInUrl">LinkedIn Profile (optional)</label>
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

            <div>
              <label htmlFor="heardAboutRole">
                How did you hear about this role?
              </label>
              <select
                id="heardAboutRole"
                {...register("heardAboutRole")}
                className="w-full rounded border p-2"
              >
                <option value="">Select...</option>
                <option value="LinkedIn">LinkedIn</option>
                <option value="Company Website">Company Website</option>
                <option value="Friend">Friend</option>
                <option value="Job Board">Job Board</option>
                <option value="Other">Other</option>
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
          <div className="space-y-4">
            <h3 className="text-lg font-semibold">Review &amp; Submit</h3>

            <dl className="space-y-3">
              <div>
                <dt className="font-medium">Full Name</dt>
                <dd>{displayValue(watchedValues.fullName)}</dd>
              </div>

              <div>
                <dt className="font-medium">Email</dt>
                <dd>{displayValue(watchedValues.email)}</dd>
              </div>

              <div>
                <dt className="font-medium">Phone</dt>
                <dd>{displayValue(watchedValues.phone)}</dd>
              </div>

              <div>
                <dt className="font-medium">Cover Letter</dt>
                <dd>{displayValue(watchedValues.coverLetter)}</dd>
              </div>

              <div>
                <dt className="font-medium">LinkedIn</dt>
                <dd>{displayValue(watchedValues.linkedInUrl)}</dd>
              </div>

              <div>
                <dt className="font-medium">Heard About Role</dt>
                <dd>{displayValue(watchedValues.heardAboutRole)}</dd>
              </div>
            </dl>
          </div>
        )}
      </div>

      {hasDraft && (
        <div className="flex justify-end">
          <AlertDialog>
          <AlertDialogTrigger
            render={
              <button
                type="button"
                className="rounded border border-red-600 px-4 py-2"
              >
                Discard Draft
              </button>
            }
          />

          <AlertDialogContent>
            <AlertDialogHeader>
              <AlertDialogTitle>
                Discard your draft?
              </AlertDialogTitle>

              <AlertDialogDescription>
                Your saved application progress will be permanently deleted.
              </AlertDialogDescription>
            </AlertDialogHeader>

            <AlertDialogFooter>
              <AlertDialogCancel>
                Keep draft
              </AlertDialogCancel>

              <AlertDialogAction onClick={discardDraft}>
                Discard draft
              </AlertDialogAction>
            </AlertDialogFooter>
          </AlertDialogContent>
        </AlertDialog>
        </div>
      )}

      <div className="flex justify-between">
        <button
          type="button"
          disabled={step === 1}
          onClick={() => setStep((current) => current - 1)}
          className="rounded border px-4 py-2 disabled:opacity-50"
        >
          Back
        </button>

        {step < 3 ? (
          <button
            type="button"
            onClick={nextStep}
            className="rounded bg-blue-600 px-4 py-2 text-white"
          >
            Next
          </button>
        ) : (
          <button
            type="submit"
            disabled={mutation.isPending}
            className="rounded bg-green-600 px-4 py-2 text-white disabled:opacity-50"
          >
            {mutation.isPending ? "Submitting..." : "Submit Application"}
          </button>
        )}
      </div>
    </form>
  );
}
export default ApplicationWizard;