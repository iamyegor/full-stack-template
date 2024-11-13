import embedLocale from "@/features/i18n/utils/embedLocale";
import getLocale from "@/features/i18n/utils/getLocales";
import { redirect as nextRedirect } from "next/navigation";

export default async function redirect(href: string) {
    const locale = await getLocale();
    const localizedHref = embedLocale(href, locale);
    return nextRedirect(localizedHref);
}
