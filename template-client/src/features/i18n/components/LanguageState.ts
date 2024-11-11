"use client";

import useLangStore from "@/lib/zustand/useLangStore";
import Language from "@/types/Language";
import { useEffect } from "react";

export default function LanguageState({ lang: newLang }: { lang: Language }) {
    const { setLang, lang } = useLangStore();

    useEffect(() => {
        if (newLang !== lang) {
            setLang(newLang);
        }
    }, [newLang]);

    return null;
}
