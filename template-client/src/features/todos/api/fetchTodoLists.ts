import Language from "@/features/languages/types/Language";

export default async function fetchTodoLists(locale: Language) {
    const response = await fetch(`${process.env.strapi}/home-page?locale=${locale}`, {
        cache: "no-store",
    });

    if (!response.ok) {
        throw new Error("Failed to fetch todo lists");
    }

    return response.json();
}
