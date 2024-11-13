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

export default function embedLocale(href: string, locale: string): string {
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
