import Todo from "@/src/types/Todo";

export default interface PagedTodoResponse {
    todos: Todo[];
    nextPage: number | null;
}
