import type { FileResponse } from "../api/dtos/fileApiDtos";
import { formatBytes } from "../utils/formatBytes";
import { formatDate } from "../utils/formatDate";
import type { FileModel } from "./fileModels";

export const mapFileResponseToModel = (dto: FileResponse): FileModel => {
    const uploadedAt = dto.uploadedAt ? new Date(dto.uploadedAt) : null;

    return {
        fileName: dto.fileName,
        sizeInBytes: dto.sizeInBytes,
        uploadedAt,
        sizeFormatted: formatBytes(dto.sizeInBytes),
        uploadedAtFormatted: formatDate(uploadedAt)
    };
};