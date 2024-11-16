import commonErrors from "@/data/commonErrors";
import sendAddTodoRequest from "@/features/todos/api/sendAddTodoRequest";
import PagedTodoResponse from "@/features/todos/types/PagedTodoResponse";
import { Todo } from "@/features/todos/types/Todo";
import ServerErrorResponse from "@/types/errors/ServerErrorResponse";
import { InfiniteData, useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError } from "axios";

export default function useAddPagedTodo({
    setErrorMessage,
}: {
    setErrorMessage: (message: string | null) => void;
}) {
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
        onError: (error, __, context) => {
            queryClient.setQueryData([queryKey, { page: 1 }], context?.previousTodos);

            const axiosError = error as AxiosError<ServerErrorResponse>;
            if (
                axiosError.response?.data.errorCode &&
                axiosError.response.data.errorCode in commonErrors
            ) {
                setErrorMessage(
                    commonErrors[axiosError.response.data.errorCode as keyof typeof commonErrors]
                );
            } else {
                setErrorMessage(commonErrors["unexpected.error"]);
            }
        },
        onSettled: () => {
            queryClient.invalidateQueries({ queryKey });
        },
    });
}
