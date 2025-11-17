import { DataGrid, type GridRenderCellParams } from "@mui/x-data-grid";
import DeleteIcon from "@mui/icons-material/Delete";
import DownloadIcon from "@mui/icons-material/Download";
import { Box, Button } from "@mui/material";
import { useTranslation } from "react-i18next";
import type { FileModel } from "../../models/fileModels";

type Props = {
  files: FileModel[];
  onDelete: (fileName: string) => void;
  deletingFileName: string | null;
  onDownload: (fileName: string) => void;
};

export const FileList: React.FC<Props> = ({
  files,
  onDelete,
  deletingFileName,
  onDownload
}) => {
  const { t } = useTranslation();

  const columns = [
    { field: "fileName", headerName: t("FILE_NAME"), flex: 1 },
    { field: "sizeFormatted", headerName: t("FILE_SIZE"), flex: 1 },
    { field: "uploadedAtFormatted", headerName: t("UPOADED_DATE"), flex: 1 },
    {
      field: "actions",
      headerName: t("ACTIONS"),
      flex: 1,
      renderCell: (params: GridRenderCellParams) => (
        <Box display="flex" gap={1}>
          {/* Download */}
          <Button
            color="primary"
            variant="contained"
            startIcon={<DownloadIcon />}
            onClick={() => onDownload(params.row.fileName)}
          >
            {t("download")}
          </Button>
          <Button
            color="error"
            startIcon={<DeleteIcon />}
            onClick={() => onDelete(params.row.fileName)}
            disabled={deletingFileName === params.row.fileName}
          />
        </Box>
      ),
    },
  ];

  return (
    <DataGrid rows={files} columns={columns} getRowId={(row) => row.fileName} />
  );
};
