"use server";

import { Todo } from "@/features/todos/types/Todo";

export default async function fetchTodos(): Promise<Todo[]> {
    const response = await fetch(process.env.api + "/todos");
    return (await response.json()) as Todo[];
}
