import Providers from "@/lib/Providers";
import { Inter } from "next/font/google";
import React from "react";
import { GoogleAnalytics } from "@next/third-parties/google";
import "./globals.css";
import Script from "next/script";

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
            </body>
            <GoogleAnalytics gaId="G-SDHY5BQ9YH" />
            <Script
                async
                src="https://stats.fullstacktemplate.ru/script.js"
                data-website-id="54483bc1-f65f-4df1-b075-43fb798be847"
            />
        </html>
    );
}
