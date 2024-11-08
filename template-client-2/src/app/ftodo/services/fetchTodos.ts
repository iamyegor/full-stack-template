"use server";

import Todo from "@/src/types/Todo";

export default async function fetchTodos(): Promise<Todo[]> {
    const response = await fetch("todos", { next: { revalidate: 60 } });
    return (await response.json()) as Todo[];
}
