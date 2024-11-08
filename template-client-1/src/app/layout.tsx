import { Inter } from "next/font/google";
import React from "react";
import "./globals.css";
import Providers from "@/providers/Providers";

const interFont = Inter({
    subsets: ["cyrillic"],
    variable: "--font-inter",
    weight: ["400", "500", "600", "700"],
});

export default function RootLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <html className={`${interFont.variable}`}>
            <body className={`antialiased`}>
                <Providers>{children}</Providers>
            </body>
        </html>
    );
}
