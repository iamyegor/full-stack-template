import type { MetadataRoute } from "next";

export default function sitemap(): MetadataRoute.Sitemap {
    const siteUrl = process.env.url as string;

    return [
        {
            url: siteUrl,
            lastModified: new Date(),
            changeFrequency: "always",
            priority: 1,
        },
    ];
}
