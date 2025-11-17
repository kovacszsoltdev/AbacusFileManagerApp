import type { FileResponse } from "./dtos/fileApiDtos";
import { api } from "./client";

export class FileService {

    async upload(file: File): Promise<void> {
        const formData = new FormData();
        formData.append("file", file);

        await api.post("/files/upload", formData, {
            headers: { "Content-Type": "multipart/form-data" }
        });
    }

    async list(): Promise<FileResponse[]> {
        const response = await api.get<FileResponse[]>("/files");
        return response.data;
    }

    async delete(fileName: string): Promise<void> {
        await api.delete(`/files/${encodeURIComponent(fileName)}`);
    }

    async download(fileName: string): Promise<void> {
        const response = await api.get(`/files/${fileName}`, {
            responseType: "blob"
        });

        const url = window.URL.createObjectURL(response.data);
        const link = document.createElement("a");
        link.href = url;
        link.download = fileName;
        link.click();
        window.URL.revokeObjectURL(url);
    };
}

export const fileService = new FileService();
