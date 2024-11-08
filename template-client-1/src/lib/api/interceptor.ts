import authApi from "@/src/lib/api/authApi";
import ServerErrorResponse from "@/src/types/errors/ServerErrorResponse";
import EmptyResult from "@/src/types/results/EmptyResult";
import { AxiosError, AxiosRequestConfig, AxiosResponse } from "axios";

interface CustomAxiosRequestConfig extends AxiosRequestConfig {
    _retry?: boolean;
    _refreshToken?: boolean;
}

export default async function interceptor(
    api: (config: CustomAxiosRequestConfig) => Promise<AxiosResponse<any, any>>,
    error: AxiosError<ServerErrorResponse>
): Promise<any> {
    const originalRequest = error.config as CustomAxiosRequestConfig;

    try {
        if (originalRequest._retry) {
            return Promise.reject(error);
        }
        originalRequest._retry = true;

        if (error.response?.data?.errorCode === "device.id.is.invalid") {
            await issueNewDeviceId();
        } else if (error.response?.status === 401) {
            const refreshResult = await refreshToken();
            if (refreshResult.isFailure) {
                return api(originalRequest);
            }
        } else {
            return Promise.reject(error);
        }

        return api(originalRequest);
    } catch (newRequestError) {
        return Promise.reject(newRequestError);
    }
}

async function refreshToken(): Promise<EmptyResult> {
    try {
        await authApi.post("refresh-access-token");
        return EmptyResult.Ok();
    } catch (refreshError) {
        return EmptyResult.Fail("Token refresh failed");
    }
}

async function issueNewDeviceId(): Promise<any> {
    await authApi.post("issue-device-id");
}
