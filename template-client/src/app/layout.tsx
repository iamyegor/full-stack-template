import GDPRBanner from "@/components/GDPRBanner/GDPRBanner";
import Providers from "@/components/Providers/Providers";
import { Inter } from "next/font/google";
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
                <GDPRBanner />
            </body>
        </html>
    );
}
