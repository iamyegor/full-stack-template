"use server";

import api from "@/src/lib/api/api";

export default async function sendMarkTodoCompletedRequest(todoId: number): Promise<void> {
    await api.post("todos/mark-completed", { todoId });
}
