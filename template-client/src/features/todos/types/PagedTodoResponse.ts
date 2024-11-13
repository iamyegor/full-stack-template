import Todo from "@/features/todos/types/Todo";

export default interface PagedTodoResponse {
    todos: Todo[];
    nextPage: number | null;
}
