import sendChangeCompletionStatusRequest from "@/features/todos/api/sendMarkTodoCompletedRequest";
import PagedTodoResponse from "@/features/todos/types/PagedTodoResponse";
import { InfiniteData, useMutation, useQueryClient } from "@tanstack/react-query";

const queryKey = ["todos-paged"];

export default function usePagedTodoChangeCompletion() {
    const queryClient = useQueryClient();

    const markTodoCompletedMutation = useMutation({
        mutationFn: sendChangeCompletionStatusRequest,
        onMutate: async ({ todoId, completed }: { todoId: number; completed: boolean }) => {
            await queryClient.cancelQueries({ queryKey });

            const previousData =
                queryClient.getQueryData<InfiniteData<PagedTodoResponse>>(queryKey);

            queryClient.setQueryData<InfiniteData<PagedTodoResponse>>(queryKey, (data) => {
                if (!data) return previousData;

                return {
                    ...data,
                    pages: data.pages.map((page) => ({
                        ...page,
                        todos: page.todos.map((todo) =>
                            todo.id === todoId ? { ...todo, completed } : todo
                        ),
                    })),
                };
            });

            return { previousData };
        },
        onError: (_, __, context) => {
            if (context?.previousData) queryClient.setQueryData(queryKey, context?.previousData);
        },
        onSettled: () => queryClient.invalidateQueries({ queryKey }),
    });

    return markTodoCompletedMutation.mutate;
}
