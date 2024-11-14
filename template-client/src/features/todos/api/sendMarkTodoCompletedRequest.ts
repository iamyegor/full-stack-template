import api from "@/lib/api/api";

export default async function sendChangeCompletionStatusRequest({
    todoId,
    completed,
}: {
    todoId: number;
    completed: boolean;
}) {
    await api.post(`/todos/${todoId}/change-completion-status`, { completed });
}
