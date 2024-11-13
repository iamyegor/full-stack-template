"use client";

import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select";
import { usePathname, useRouter } from "next/navigation";

type Language = {
    code: string;
    name: string;
    flag: string;
};

const languages: Language[] = [
    { code: "en", name: "English", flag: "🇺🇸" },
    { code: "ru", name: "Русский", flag: "🇷🇺" },
];

export default function LanguageSwitcher() {
    const router = useRouter();
    const pathname = usePathname();
    const currentLang = pathname.startsWith("/ru") ? "ru" : "en";

    function handleLanguageChange(newLang: string) {
        router.push("/" + newLang);
    }

    return (
        <div className="absolute right-20 top-5 flex items-center gap-2">
            <Select value={currentLang} onValueChange={handleLanguageChange}>
                <SelectTrigger className="w-[65px] bg-white border-blue-600">
                    <SelectValue />
                </SelectTrigger>
                <SelectContent>
                    {languages.map((lang) => (
                        <SelectItem key={lang.code} value={lang.code} className="cursor-pointer">
                            <span className="flex items-center gap-2">
                                <span>{lang.flag}</span>
                                {/* <span>{lang.name}</span> */}
                            </span>
                        </SelectItem>
                    ))}
                </SelectContent>
            </Select>
        </div>
    );
}
