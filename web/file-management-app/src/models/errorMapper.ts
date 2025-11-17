import type { AppError } from "./errorModel";

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const mapProblemDetailsToAppError = (data: any): AppError => {
    return {
        status: data.status,
        errorCode: data.errorCode,
        message: data.detail || data.title || "Unknown error",
        validation: data.details?.validation,
        traceId: data.traceId
    };
};
