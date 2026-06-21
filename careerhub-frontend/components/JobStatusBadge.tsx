import { Badge } from "@/components/ui/badge";
import { cn } from "@/lib/utils";
import { EmploymentType } from "@/types";

interface JobStatusBadgeProps {
  employmentType?: EmploymentType;
  isActive?: boolean;
}

export function JobStatusBadge({
  employmentType,
  isActive,
}: JobStatusBadgeProps) {
  if (employmentType) {
    const classes: Record<EmploymentType, string> = {
      FullTime: "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200",
      PartTime: "bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-200",
      Contract: "bg-purple-100 text-purple-800 dark:bg-purple-900 dark:text-purple-200",
      Internship: "bg-orange-100 text-orange-800 dark:bg-orange-900 dark:text-orange-200",
    };

    return (
      <Badge className={cn(classes[employmentType])}>
        {employmentType}
      </Badge>
    );
  }

  if (isActive === false) {
    return (
      <Badge variant="destructive">
        Closed
      </Badge>
    );
  }

  return null;
}