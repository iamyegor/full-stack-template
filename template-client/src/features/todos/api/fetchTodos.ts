"use server";

import Todo from "@/features/todos/types/Todo";

export default async function fetchTodos(): Promise<Todo[]> {
    const response = await fetch(process.env.api + "todos", { next: { revalidate: 60 } });
    return (await response.json()) as Todo[];
}
