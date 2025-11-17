import { Button } from "@mui/material";
import UploadIcon from "@mui/icons-material/Upload";
import { useTranslation } from "react-i18next";

type Props = {
    onSelected: (file: File) => void;
    disabled?: boolean;
};

export const FileUpload: React.FC<Props> = ({ onSelected, disabled }) => {
    const { t } = useTranslation();

    return (
        <label htmlFor="file-upload">
            <input
                id="file-upload"
                type="file"
                multiple={false}
                style={{ display: "none" }}
                onChange={(e) => {
                    if (e.target.files && e.target.files[0]) {
                        onSelected(e.target.files[0]);
                    }
                }}
            />
            <Button
                component="span"
                variant="contained"
                startIcon={<UploadIcon />}                
                disabled={disabled}
            >
                {t("UPLOAD_FILE")}
            </Button>
        </label>
    );
};
