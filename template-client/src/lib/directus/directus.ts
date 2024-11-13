import { createDirectus, rest } from "@directus/sdk";

const directus = createDirectus(process.env.directus!).with(rest());

export default directus;
