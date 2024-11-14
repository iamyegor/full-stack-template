import sendAddTodoRequest from "@/features/todos/api/sendAddTodoRequest";
import PagedTodoResponse from "@/features/todos/types/PagedTodoResponse";
import { Todo } from "@/features/todos/types/Todo";
import { InfiniteData, useMutation, useQueryClient } from "@tanstack/react-query";

export default function useAddPagedTodo() {
    const queryKey = ["todos-paged"];
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: sendAddTodoRequest,
        onMutate: async (text) => {
            await queryClient.cancelQueries({ queryKey });

            const previousTodos =
                queryClient.getQueryData<InfiniteData<PagedTodoResponse>>(queryKey);

            const optimisticTodo: Todo = {
                id: Date.now(),
                title: text,
                completed: false,
                createdAt: new Date().toISOString(),
            };

            queryClient.setQueryData<InfiniteData<PagedTodoResponse>>(queryKey, (data) => {
                if (!data) return previousTodos;

                return {
                    ...data,
                    pages: data.pages.map((page) => ({
                        ...page,
                        todos: [optimisticTodo, ...page.todos],
                    })),
                };
            });

            return { previousTodos };
        },
        onError: (_, __, context) => {
            queryClient.setQueryData([queryKey, { page: 1 }], context?.previousTodos);
        },
        onSettled: () => {
            queryClient.invalidateQueries({ queryKey });
        },
    });
}
