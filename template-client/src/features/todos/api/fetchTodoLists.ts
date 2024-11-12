import Language from "@/features/languages/types/Language";
import directus from "@/lib/directus";
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

    console.log({ finite, infinite });

    return { finite, infinite };
}
