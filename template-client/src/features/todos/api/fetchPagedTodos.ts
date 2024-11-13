import PagedTodoResponse from "@/features/todos/types/PagedTodoResponse";
import api from "@/lib/api/api";

export default async function fetchPagedTodos({ pageParam = 1 }) {
    console.log("process.env.api", process.env.api);
    console.log("process.env.auth", process.env.auth);
    console.log("process.env.cms", process.env.cms);
    const { data } = await api.get<PagedTodoResponse>(`todos/paged/${pageParam}`);
    return data;
}
