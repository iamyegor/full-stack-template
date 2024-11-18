import svgsConfig from "./svgs.config.js";

/** @type {import('next').NextConfig} */
const nextConfig = {
    env: {
        api:
            process.env.NODE_ENV === "development"
                ? "http://localhost:5026"
                : "FILL_ME_IN",
        authApi:
            process.env.NODE_ENV === "development"
                ? "http://localhost:5029"
                : "FILL_ME_IN",
        cms:
            process.env.NODE_ENV === "development"
                ? "http://localhost:8055"
                : "FILL_ME_IN",
    },
    images: {
        domains: ["localhost"],
    },
    experimental: {
        turbo: {
            rules: {
                "*.svg": {
                    loaders: ["@svgr/webpack"],
                    as: "*.js",
                },
            },
        },
    },
    reactStrictMode: false,
    async rewrites() {
        return [
            {
                source: "/ingest/static/:path*",
                destination: "https://eu-assets.i.posthog.com/static/:path*",
            },
            {
                source: "/ingest/:path*",
                destination: "https://eu.i.posthog.com/:path*",
            },
            {
                source: "/ingest/decide",
                destination: "https://eu.i.posthog.com/decide",
            },
        ];
    },
    skipTrailingSlashRedirect: true,
    webpack: svgsConfig,
};

export default nextConfig;
