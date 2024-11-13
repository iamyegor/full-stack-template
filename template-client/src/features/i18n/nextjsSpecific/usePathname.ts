import { useLocale } from "@/features/i18n/hooks/useLocales";
import { usePathname as useNextPathname } from "next/navigation";
import removeLocale from "@/features/i18n/utils/removeLocale";

export default function usePathname() {
    const locale = useLocale();
    const pathname = useNextPathname();

    return removeLocale(pathname, locale);
}
