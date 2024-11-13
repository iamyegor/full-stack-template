import { useLocale } from "@/features/i18n/hooks/useLocales";
import embedLocale from "@/features/i18n/utils/embedLocale";
import { useRouter as useNextRouter } from "next/navigation";

export default function useRouter() {
    const router = useNextRouter();
    const locale = useLocale();

    return {
        ...router,
        push: (href: string) => router.push(embedLocale(href, locale)),
        replace: (href: string) => router.replace(embedLocale(href, locale)),
        prefetch: (href: string) => router.prefetch(embedLocale(href, locale)),
    };
}
