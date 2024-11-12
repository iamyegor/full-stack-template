import svgsConfig from "./svgs.config.js";
import createNextIntlPlugin from "next-intl/plugin";

const withNextIntl = createNextIntlPlugin();

/** @type {import('next').NextConfig} */
const nextConfig = {
    env: {
        api: process.env.NODE_ENV === "development" ? "http://localhost:5026/" : "FILL_ME_IN",
        authApi: process.env.NODE_ENV === "development" ? "http://localhost:5029/" : "FILL_ME_IN",
        domain: "FILL_ME_IN",
        DB_HOST: process.env.NODE_ENV === "development" ? "localhost" : process.env.DB_HOST,
        DB_USER: process.env.NODE_ENV === "development" ? "postgres" : process.env.DB_USER,
        DB_PASSWORD: process.env.NODE_ENV === "development" ? "yegor" : process.env.DB_PASSWORD,
        DB_NAME: process.env.NODE_ENV === "development" ? "template_db" : process.env.DB_NAME,
        cms: process.env.NODE_ENV === "development" ? "http://localhost:1337/api" : process.env.STRAPI,
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

export default withNextIntl(nextConfig);

// export default nextConfig;
