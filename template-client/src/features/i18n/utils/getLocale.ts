import { cookies, headers } from "next/headers";
import { defaultLocale, locales } from "@/features/i18n/data/locales";
import { NextRequest } from "next/dist/server/web/spec-extension/request";

export default async function getLocale(request?: NextRequest) {
    if (!request) {
        const headersList = await headers();
        const url = headersList.get("x-url") || headersList.get("x-invoke-path");

        console.log("url", url);

        if (url) {
            const segments = url.split("/").filter(Boolean);
            const routeLocale = segments[0];

            if (routeLocale && locales.includes(routeLocale)) {
                return routeLocale;
            }
        }
    }

    let cookiesList: any = await cookies();
    let headersList: any = await headers();
    if (request) {
        cookiesList = request.cookies;
        headersList = request.headers;
    }

    const preferredLocale = cookiesList.get("preferred-locale")?.value;
    if (preferredLocale && locales.includes(preferredLocale)) {
        return preferredLocale;
    }

    const acceptLanguage = headersList.get("accept-language");
    if (acceptLanguage) {
        const browserLocale = acceptLanguage.split(",")[0].split("-")[0];

        if (locales.includes(browserLocale)) {
            return browserLocale;
        }
    }

    return defaultLocale;
}
