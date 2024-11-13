"use server";

import Language from "@/features/i18n/types/Language";
import getDirectusClient from "@/lib/directus/getDirectusClient";
// import directus from "@/lib/directus/directus";
import { readItems } from "@directus/sdk";

interface HomePage {
    id: number;
    todoLists: {
        id: number;
        translations: {
            title: string;
            description: string;
            buttonText: string;
        }[];
    }[];
}

export default async function fetchTodoLists(locale: Language) {
    const directus = getDirectusClient();

    const homePage = (await directus.request(
        readItems("homePage", {
            deep: {
                todoLists: {
                    translations: {
                        _filter: {
                            languages_code: { _eq: locale },
                        },
                    },
                },
            },
            fields: ["*", { todoLists: ["*", { translations: ["*"] }] }],
        })
    )) as never as HomePage;

    const finite = { ...homePage.todoLists[0].translations[0], id: homePage.todoLists[0].id };
    const infinite = { ...homePage.todoLists[1].translations[0], id: homePage.todoLists[1].id };

    return { finite, infinite };
}
