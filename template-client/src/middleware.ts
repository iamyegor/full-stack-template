import { locales } from "@/features/i18n/data/locales";
import getLocale from "@/features/i18n/utils/getLocales";
import { NextRequest, NextResponse } from "next/server";

export async function middleware(request: NextRequest) {
    const { pathname } = request.nextUrl;
    const desiredLocale = locales.find(
        (locale) => pathname.startsWith(`/${locale}/`) || pathname === `/${locale}`
    );

    const currentLocale = request.cookies.get("preferredLocale")?.value;
    if (desiredLocale && desiredLocale === currentLocale) {
        return NextResponse.next();
    } else if (desiredLocale) {
        const response = NextResponse.next();
        response.cookies.set("preferredLocale", desiredLocale);
        return response;
    }

    const locale = await getLocale(request);
    request.nextUrl.pathname = `/${locale}${pathname}`;

    return NextResponse.redirect(request.nextUrl);
}

export const config = {
    matcher: ["/((?!_next|ingest).*)"],
};
