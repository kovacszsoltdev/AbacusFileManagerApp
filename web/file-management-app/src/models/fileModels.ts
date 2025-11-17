export interface FileModel {
    fileName: string;
    sizeInBytes: number | null;
    uploadedAt: Date | null;
    sizeFormatted: string;
    uploadedAtFormatted: string;
}
