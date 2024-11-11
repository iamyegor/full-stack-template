import LanguageState from "@/features/i18n/components/LanguageState";
import Language from "@/types/Language";

export default async function layout({
    children,
    params,
}: {
    children: React.ReactNode;
    params: Promise<{ lang: Language }>;
}) {
    const lang = (await params).lang;

    return (
        <>
            {children}
            <LanguageState lang={lang} />
        </>
    );
}
