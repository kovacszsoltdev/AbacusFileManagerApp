import { useEffect, useState } from "react";
import { fileService } from "../api/fileService";
import { mapFileResponseToModel } from "../models/fileMapper";
import type { FileModel } from "../models/fileModels";
import type { AppError } from "../models/errorModel";
import { extractErrorCodes } from "../utils/appErrors";

export const useFiles = () => {
    const [files, setFiles] = useState<FileModel[]>([]);
    const [isLoadingList, setIsLoadingList] = useState(true);
    const [isUploading, setIsUploading] = useState(false);
    const [deletingFileName, setDeletingFileName] = useState<string | null>(null);
    const [errors, setErrors] = useState<string[] | null>(null);

    const load = async () => {
        try {
            setIsLoadingList(true);
            const dtoList = await fileService.list();
            setFiles(dtoList.map(mapFileResponseToModel));
            setErrors(null);
        } catch(err) {            
            setErrors(extractErrorCodes(err as AppError));
        } finally {
            setIsLoadingList(false);
        }
    };

    useEffect(() => {
        load();
    }, []);

    const upload = async (file: File) => {
        try {
            setIsUploading(true);
            await fileService.upload(file);
            await load();
        } catch(err) {            
            setErrors(extractErrorCodes(err as AppError));
        } finally {
            setIsUploading(false);
        }
    };

    const remove = async (fileName: string) => {
        try {
            setDeletingFileName(fileName);
            await fileService.delete(fileName);
            await load();
        } catch(err) {            
            setErrors(extractErrorCodes(err as AppError));
        } finally {
            setDeletingFileName(null);
        }
    };

    const download = async (fileName: string) => {
        try {
            await fileService.download(fileName);
        } catch(err) {
            setErrors(extractErrorCodes(err as AppError));
        }
    };

    return {
        files,
        isLoadingList,
        isUploading,
        deletingFileName,
        errors,
        upload,
        remove,
        download
    };
};