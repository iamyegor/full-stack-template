export default function removeLocale(path: string, locale: string): string {
    if (!path) return path;

    if (path.startsWith(`/${locale}/`)) {
        return path.substring(`/${locale}`.length);
    }

    if (path.startsWith(`/${locale}`)) {
        return path.substring(`/${locale}`.length) || "/";
    }

    return path;
}