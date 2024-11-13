import { createDirectus, rest } from "@directus/sdk";

export default function getDirectusClient() {
    return createDirectus(process.env.cms!).with(rest());
}
