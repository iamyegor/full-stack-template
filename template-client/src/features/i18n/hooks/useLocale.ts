import { defaultLocale, locales } from "@/features/i18n/data/locales";

function getCookie(name: string) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) {
        return parts.pop()?.split(";").shift();
    }

    return null;
}

export function useLocale() {
    const preferredLocale = getCookie("preferred-locale");
    if (preferredLocale && locales.includes(preferredLocale)) {
        return preferredLocale;
    }

    const browserLocale = navigator.language.split("-")[0];
    if (locales.includes(browserLocale)) {
        return browserLocale;
    }

    return defaultLocale;
}
