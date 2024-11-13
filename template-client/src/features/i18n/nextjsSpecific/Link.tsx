"use client";

import { useLocale } from "@/features/i18n/hooks/useLocales";
import embedLocale from "@/features/i18n/utils/embedLocale";
import NextLink from "next/link";
import { AnchorHTMLAttributes } from "react";

interface LinkProps extends AnchorHTMLAttributes<HTMLAnchorElement> {
    href: string;
    children?: React.ReactNode;
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
