import axios from "axios";
import { mapProblemDetailsToAppError } from "../models/errorMapper";
import type { AppError } from "../models/errorModel";

const baseURL = import.meta.env.VITE_API_BASE_URL;

export const api = axios.create({
    baseURL: baseURL + "/api",
});

api.interceptors.response.use(
    response => response,
    error => {
        if (error.response?.data) {
            const appError = mapProblemDetailsToAppError(error.response.data);
            return Promise.reject(appError);
        }
        return Promise.reject({
            message: "An unexpected error occurred.",
            errorCode: "UNEXPECTED",
            status: 0         
        } as AppError);
    }
);