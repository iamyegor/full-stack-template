"use client";

import { useLocale } from "@/features/i18n/hooks/useLocales";
import getLocale from "@/features/i18n/utils/getLocales";
import NextLink from "next/link";
import { AnchorHTMLAttributes } from "react";

interface LinkProps extends AnchorHTMLAttributes<HTMLAnchorElement> {
    href: string;
    children?: React.ReactNode;
}

function isExternalUrl(url: string): boolean {
    return /^([a-z][a-z\d+\-.]*:)?\/\//i.test(url);
}

function isSpecialProtocol(url: string): boolean {
    return (
        url.startsWith("mailto:") ||
        url.startsWith("tel:") ||
        url.startsWith("sms:") ||
        url.startsWith("ftp:")
    );
}

function isHashLink(url: string): boolean {
    return url.startsWith("#");
}

function embedLocale(href: string, locale: string): string {
    if (!href) return href;

    if (isExternalUrl(href) || isSpecialProtocol(href) || isHashLink(href)) {
        return href;
    }

    if (href.startsWith("/")) {
        const segments = href.split("/");
        if (segments[1] === locale) {
            return href;
        }
        return `/${locale}${href}`;
    }

    if (href.startsWith(`${locale}/`)) {
        return href;
    }
    return `${locale}/${href}`;
}

export default function Link({ href, children, ...rest }: LinkProps) {
    const locale = useLocale();
    const localizedHref = embedLocale(href, locale);

    return (
        <NextLink href={localizedHref} {...rest}>
            {children}
        </NextLink>
    );
}
