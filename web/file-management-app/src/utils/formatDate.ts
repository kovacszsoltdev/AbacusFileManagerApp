export const formatDate = (date: Date | null) => {
    if (!date) {
        return "-";
    }

    return date.toLocaleString(navigator.language, {
        year: "numeric",
        month: "2-digit",
        day: "2-digit",
        hour: "2-digit",
        minute: "2-digit"
    });
};
