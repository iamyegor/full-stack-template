import PagedTodoResponse from "@/features/todos/types/PagedTodoResponse";
import api from "@/lib/api/api";

export default async function fetchPagedTodos({ pageParam = 1 }) {
    const { data } = await api.get<PagedTodoResponse>(`/todos/paged/${pageParam}`);
    return data;
}
