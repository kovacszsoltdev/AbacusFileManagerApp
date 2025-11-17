export interface ValidationErrors {
    [field: string]: string[];
}

export interface AppError {
    status: number;
    errorCode: string;
    message: string;
    validation?: ValidationErrors;
    traceId?: string;
}
