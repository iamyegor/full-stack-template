import commonErrors from "@/data/commonErrors";
import sendChangeCompletionStatusRequest from "@/features/todos/api/sendMarkTodoCompletedRequest";
import { Todo } from "@/features/todos/types/Todo";
import ServerErrorResponse from "@/types/errors/ServerErrorResponse";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError } from "axios";
import getCommonError from "@/utils/errors/getCommonError";

export default function useTodoChangeCompletion({
    setErrorMessage,
}: {
    setErrorMessage: (errorMessage: string | null) => void;
}) {
    const queryKey = ["todos"];
    const queryClient = useQueryClient();

    const changeCompletionStatusMutation = useMutation({
        mutationFn: sendChangeCompletionStatusRequest,
        onMutate: async ({ todoId, completed }) => {
            await queryClient.cancelQueries({ queryKey: ["todos"] });

            const previousData = queryClient.getQueryData<Todo[]>(["todos"]);

            queryClient.setQueryData<Todo[]>(["todos"], (prev = []) =>
                prev.map((todo) => (todo.id === todoId ? { ...todo, completed } : todo))
            );

            return { previousData };
        },
        onError: (error, __, context) => {
            queryClient.setQueryData(queryKey, context?.previousData);
            setErrorMessage(getCommonError(error as AxiosError<ServerErrorResponse>));
        },
        onSettled: () => queryClient.invalidateQueries({ queryKey }),
    });

    return { changeCompletionStatus: changeCompletionStatusMutation.mutate };
}
