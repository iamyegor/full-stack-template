import commonErrors from "@/data/commonErrors";
import sendChangeCompletionStatusRequest from "@/features/todos/api/sendMarkTodoCompletedRequest";
import PagedTodoResponse from "@/features/todos/types/PagedTodoResponse";
import ServerErrorResponse from "@/types/errors/ServerErrorResponse";
import { InfiniteData, useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError } from "axios";
import getCommonError from "@/utils/errors/getCommonError";

const queryKey = ["todos-paged"];

export default function usePagedTodoChangeCompletion({
    setErrorMessage,
}: {
    setErrorMessage: (message: string | null) => void;
}) {
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
        onError: (error, __, context) => {
            queryClient.setQueryData(queryKey, context?.previousData);
            setErrorMessage(getCommonError(error as AxiosError<ServerErrorResponse>));
        },
        onSettled: () => queryClient.invalidateQueries({ queryKey }),
    });

    return markTodoCompletedMutation;
}
