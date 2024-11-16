import sendAddTodoRequest from "@/features/todos/api/sendAddTodoRequest";
import { Todo } from "@/features/todos/types/Todo";
import ServerErrorResponse from "@/types/errors/ServerErrorResponse";
import getCommonError from "@/utils/errors/getCommonError";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError } from "axios";

export default function useAddTodo({
    setErrorMessage,
}: {
    setErrorMessage: (message: string | null) => void;
}) {
    const queryClient = useQueryClient();

    const mutation = useMutation({
        mutationFn: (text: string) => sendAddTodoRequest(text),
        onMutate: async (newTodoText) => {
            await queryClient.cancelQueries({ queryKey: ["todos"] });

            const previousTodos = queryClient.getQueryData<Todo[]>(["todos"]);

            const optimisticTodo: Todo = {
                id: Date.now(),
                title: newTodoText,
                completed: false,
                createdAt: new Date().toISOString(),
            };

            queryClient.setQueryData<Todo[]>(["todos"], (old = []) => [optimisticTodo, ...old]);

            return { previousTodos };
        },
        onError: (error, __, context) => {
            queryClient.setQueryData(["todos"], context?.previousTodos);
            setErrorMessage(getCommonError(error as AxiosError<ServerErrorResponse>));
        },
        onSettled: () => {
            queryClient.invalidateQueries({ queryKey: ["todos"] });
        },
    });

    return { addTodo: mutation };
}
