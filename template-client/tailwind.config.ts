import type { Config } from "tailwindcss";

const config: Config = {
    darkMode: ["class"],
    content: [
        "./src/pages/**/*.{js,ts,jsx,tsx,mdx}",
        "./src/components/**/*.{js,ts,jsx,tsx,mdx}",
        "./src/app/**/*.{js,ts,jsx,tsx,mdx}",
        "./src/features/**/*.{js,ts,jsx,tsx,mdx}",
    ],
    theme: {
        extend: {
            fontFamily: {
                sans: "var(--font-inter)",
            },
            screens: {
                xs: "420px",
                sm: "640px",
                md: "768px",
                lg: "1024px",
                xl: "1280px",
            },
            borderRadius: {
                lg: "var(--radius)",
                md: "calc(var(--radius) - 2px)",
                sm: "calc(var(--radius) - 4px)",
            },
            colors: {},
        },
        container: {
            center: true,
            padding: "20px",
            screens: {
                sm: "600px",
                md: "768px",
                lg: "1024px",
                xl: "1200px",
            },
        },
    },
    plugins: [require("tailwindcss-animate")],
};
export default config;
