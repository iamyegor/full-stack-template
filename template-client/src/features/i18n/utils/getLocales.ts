import { cookies, headers } from "next/headers";
import { defaultLocale, locales } from "@/features/i18n/data/locales";
import { NextRequest } from "next/dist/server/web/spec-extension/request";

export default async function getLocale(request?: NextRequest) {
    let cookiesList: any = await cookies();
    let headersList: any = await headers();
    if (request) {
        cookiesList = request.cookies;
        headersList = request.headers;
    }

    const preferredLocale = cookiesList.get("preferredLocale")?.value;
    if (preferredLocale && locales.includes(preferredLocale)) {
        return preferredLocale;
    }

    const acceptLanguage = headersList.get("acceptLanguage");
    if (acceptLanguage) {
        const browserLocale = acceptLanguage.split(",")[0].split("-")[0];

        if (locales.includes(browserLocale)) {
            return browserLocale;
        }
    }

    return defaultLocale;
}
