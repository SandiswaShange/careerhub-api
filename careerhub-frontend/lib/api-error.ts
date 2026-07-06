export class ApiError extends Error {
  code: number;
  detail?: string;
  fields?: Record<string, string[]>;

  constructor(
    code: number,
    message: string,
    detail?: string,
    fields?: Record<string, string[]>
  ) {
    super(detail ?? message);

    this.name = "ApiError";
    this.code = code;
    this.detail = detail;
    this.fields = fields;
  }

  get isValidation() {
    return this.code === 422 && !!this.fields;
  }
}

export async function parseApiError(res: Response) {
  try {
    const body = await res.json();

    return new ApiError(
      res.status,
      body.title ?? "Request failed",
      body.detail,
      body.errors
    );
  } catch {
    return new ApiError(
      res.status,
      res.statusText || "Request failed"
    );
  }
}