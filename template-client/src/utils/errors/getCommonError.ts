import { AxiosError } from "axios";
import ServerErrorResponse from "@/types/errors/ServerErrorResponse";
import commonErrors from "@/data/commonErrors";

export default function getCommonError(error: AxiosError<ServerErrorResponse>) {
    if (
        error.response?.data.errorCode &&
        error.response.data.errorCode in commonErrors
    ) {
        return commonErrors[error.response.data.errorCode as keyof typeof commonErrors];
    }
    return commonErrors["unexpected.error"];
} 