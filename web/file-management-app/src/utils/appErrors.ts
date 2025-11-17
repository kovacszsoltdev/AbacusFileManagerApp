import type { AppError } from "../models/errorModel";

export const extractErrorCodes = (error: AppError): string[] => {    
    if (error.validation) {
        const codes: string[] = [];

        for (const field of Object.keys(error.validation)) {
            const fieldErrors = error.validation[field];
            codes.push(...fieldErrors);
        }

        return codes;
    }

    return [error.errorCode];
};