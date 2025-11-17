import { Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Button } from "@mui/material";

type Props = {
    open: boolean;
    title: string;
    message: string;
    confirmText?: string;
    confirmInProgressText?: string;
    cancelText?: string;
    loading?: boolean;
    onCancel: () => void;
    onConfirm: () => void;
};

export const ConfirmDialog = ({
    open,
    title,
    message,
    confirmText = "OK",
    confirmInProgressText = "Processing...",
    cancelText = "Cancel",
    loading = false,
    onCancel,
    onConfirm
}: Props) => {
    return (
        <Dialog open={open} onClose={onCancel}>
            <DialogTitle>{title}</DialogTitle>

            <DialogContent>
                <DialogContentText>
                    {message}
                </DialogContentText>
            </DialogContent>

            <DialogActions>
                <Button onClick={onCancel} disabled={loading}>
                    {cancelText}
                </Button>

                <Button
                    onClick={onConfirm}
                    color="error"
                    variant="contained"
                    disabled={loading}
                >
                    {loading ? confirmInProgressText : confirmText}
                </Button>
            </DialogActions>
        </Dialog>
    );
};
