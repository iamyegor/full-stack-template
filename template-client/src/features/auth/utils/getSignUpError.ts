import commonErrors from "@/data/commonErrors";
import ServerErrorResponse from "@/types/errors/ServerErrorResponse";
import { AxiosError } from "axios";

const errorDict = {
    "email.is.already.taken": "This email is already taken.",
    "max.login.attempts": "You reached the maximum number of login attempts. Wait for 10 minutes before trying again.",
    ...commonErrors,
};

export default function getSignUpError(error: AxiosError<ServerErrorResponse>) {
    if (error.response?.status === 429) {
        return errorDict["max.login.attempts"];
    } else if (error.response?.data.errorCode && error.response.data.errorCode in errorDict) {
        return errorDict[error.response.data.errorCode as keyof typeof errorDict];
    } else {
        return errorDict["unexpected.error"];
    }
}
