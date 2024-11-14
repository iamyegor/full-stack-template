import { headers } from "next/headers";

export default async function getClientIp() {
    return (
        (await headers()).get("cf-connecting-ip") ||
        (await headers()).get("x-real-ip") ||
        (await headers()).get("x-forwarded-for")?.split(",")[0] ||
        "unknown"
    );
}
