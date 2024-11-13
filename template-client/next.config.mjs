import svgsConfig from "./svgs.config.js";

/** @type {import('next').NextConfig} */
const nextConfig = {
    env: {
        api: process.env.NODE_ENV === "development" ? "http://localhost:5026/" : process.env.API,
        authApi: process.env.NODE_ENV === "development" ? "http://localhost:5029/" : process.env.AUTH_API,
        cms: process.env.NODE_ENV === "development" ? "http://localhost:8055" : process.env.DIRECTUS,
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
    webpack: svgsConfig,
};

export default nextConfig;
