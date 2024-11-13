"use client";

import { defaultLocale, locales } from "@/features/i18n/data/locales";
import { useState, useEffect } from "react";

function getCookie(name: string) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) {
        return parts.pop()?.split(";").shift();
    }

    return null;
}

export function useLocale() {
    const [locale, setLocale] = useState(defaultLocale);
    const [isClient, setIsClient] = useState(false);

    useEffect(() => {
        if (!isClient) {
            setIsClient(true);
        }

        const preferredLocale = getCookie("preferredLocale");
        if (preferredLocale && locales.includes(preferredLocale)) {
            setLocale(preferredLocale);
            return;
        }

        const browserLocale = navigator.language.split("-")[0];
        if (locales.includes(browserLocale)) {
            setLocale(browserLocale);
            return;
        }
    }, []);

    return isClient ? locale : defaultLocale;
}
