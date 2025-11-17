import { Container, Paper, Box, Typography, Stack, CircularProgress, Alert } from "@mui/material";
import { FileUpload } from "../components/FileUpload/FileUpload";
import { FileList } from "../components/FileList/FileList";
import { useFiles } from "../hooks/useFiles";
import { useState } from "react";
import { ConfirmDialog } from "../components/ConfirmDialog/ConfirmDialog";
import { t } from "i18next";

export const HomePage = () => {
    const {
        files,
        isLoadingList,
        isUploading,
        deletingFileName,
        errors,
        upload,
        remove,
        download,
    } = useFiles();
   
    const [dialogOpen, setDialogOpen] = useState(false);
    const [fileToDelete, setFileToDelete] = useState<string | null>(null);    

    const handleOnDelete = (fileName: string) => {
        setFileToDelete(fileName);
        setDialogOpen(true);
    };

    const handleConfirmDelete = async () => {
        if (!fileToDelete) return;

        await remove(fileToDelete);

        setDialogOpen(false);
        setFileToDelete(null);
    };

    const handleCancelDelete = () => {
        setDialogOpen(false);
        setFileToDelete(null);
    };

    return (
        <Container maxWidth="lg">
            <Box mt={4} mb={4}>
                <Typography variant="h4" component="h1" gutterBottom>
                    {t("HOME_PAGE_TITLE")}
                </Typography>

                <Stack spacing={3}>
                    {/* Upload card */}
                    <Paper elevation={3}>
                        <Box p={2} display="flex" alignItems="center" justifyContent="space-between">
                            <Box>
                                <Typography variant="h6">{t("UPLOAD_FILE")}</Typography>

                                <Typography variant="body2" color="text.secondary">
                                    {t("SELECT_FILE_TO_UPLOAD")}
                                </Typography>

                                <Box mt={1}>
                                    <Typography variant="caption" color="text.secondary">
                                        {t("SELECTED_FORMATS")}: <strong>.png .jpg .jpeg .pdf .txt</strong>
                                    </Typography>
                                    <br />
                                    <Typography variant="caption" color="text.secondary">
                                        {t("MAX_FILE_SIZE")}: <strong>50 MB</strong>
                                    </Typography>
                                </Box>
                            </Box>

                            <Box>
                                <FileUpload
                                    onSelected={(file) => upload(file)}
                                    disabled={isUploading}
                                />
                                {isUploading && (
                                    <Box mt={1} display="flex" justifyContent="flex-end">
                                        <CircularProgress size={20} />
                                    </Box>
                                )}
                            </Box>
                        </Box>
                    </Paper>

                    {/* Error messages */}
                    {errors && errors.length > 0 && (
                        <Box>
                            {errors.map((errorCode, index) => (
                                <Alert key={index} severity="error" sx={{ mb: 1 }}>
                                    {t(`ERRORS.${errorCode}`)}
                                </Alert>
                            ))}
                        </Box>
                    )}

                    {/* File list card */}
                    <Paper elevation={3}>
                        <Box p={2}>
                            <Typography variant="h6" gutterBottom>
                                Files
                            </Typography>

                            {isLoadingList ? (
                                <Box display="flex" justifyContent="center" py={4}>
                                    <CircularProgress />
                                </Box>
                            ) : (
                                <FileList
                                    files={files}
                                    onDelete={handleOnDelete}
                                    deletingFileName={deletingFileName}
                                    onDownload={download}
                                />
                            )}
                        </Box>
                    </Paper>
                </Stack>
                                
                <ConfirmDialog
                    open={dialogOpen}
                    title={t("DELETE_FILE")}
                    message={t("DELETE_FILE_CONFIRMATION_MESSAGE", { fileName: fileToDelete })}
                    confirmText={t("DELETE")}
                    confirmInProgressText={t("DELETING_IN_PROGRESS")}
                    cancelText={t("CANCEL")}
                    loading={deletingFileName === fileToDelete}
                    onCancel={handleCancelDelete}
                    onConfirm={handleConfirmDelete}
                />
            </Box>
        </Container>
    );
};
