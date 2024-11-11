import { Metadata } from "next";

export function generateMetadata(): Metadata {
    return {
        title: "NEXT JS Template",
        description: "A Next.js template with TypeScript and Tailwind CSS.",
        openGraph: {
            type: "website",
            // title: metaTags.openGraph.title,
            // description: metaTags.openGraph.description,
            // images: [{ url: metaTags.openGraph.image }],
            // url: metaTags.openGraph.url,
            // locale: metaTags.openGraph.locale,
        },
        twitter: {
            card: "summary_large_image",
            // site: metaTags.twitter.site,
            // title: metaTags.twitter.title,
            // description: metaTags.twitter.description,
            // images: [metaTags.twitter.image],
        },
        alternates: {
            languages: {
                en: "/en",
                ru: "/ru",
                // zh: "/zh",
                // fr: "/fr",
                // es: "/es",
                // de: "/de",
            },
        },
    };
}
