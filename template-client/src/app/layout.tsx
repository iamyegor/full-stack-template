import CookieBanner from "@/components/CookieBanner/CookieBanner";
import Providers from "@/lib/Providers";
import { GoogleAnalytics } from "@next/third-parties/google";
import { Inter } from "next/font/google";
import Script from "next/script";
import React from "react";
import "./globals.css";

const interFont = Inter({
    subsets: ["cyrillic"],
    variable: "--font-inter",
    weight: ["400", "500", "600", "700"],
});

export default async function RootLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <html className={`${interFont.variable}`} data-lt-installed="true">
            <body className={`antialiased`}>
                <Providers>{children}</Providers>
                <CookieBanner />
            </body>
            <Script
                async
                src="https://stats.fullstacktemplate.ru/script.js"
                data-website-id="54483bc1-f65f-4df1-b075-43fb798be847"
            />
        </html>
    );
}
