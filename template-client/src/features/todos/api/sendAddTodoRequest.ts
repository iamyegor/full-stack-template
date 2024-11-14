import api from "@/lib/api/api";

export default async function sendAddTodoRequest(text: string) {
    await api.post("/todos/add", { text });
}
