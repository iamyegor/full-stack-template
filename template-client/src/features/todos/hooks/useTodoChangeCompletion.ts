import sendChangeCompletionStatusRequest from "@/features/todos/api/sendMarkTodoCompletedRequest";
import { Todo } from "@/features/todos/types/Todo";
import { useMutation, useQueryClient } from "@tanstack/react-query";

export default function useTodoChangeCompletion() {
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
        onError: (_, __, context) => {
            if (context?.previousData) queryClient.setQueryData(queryKey, context?.previousData);
        },
        onSettled: () => queryClient.invalidateQueries({ queryKey }),
    });

    return { changeCompletionStatus: changeCompletionStatusMutation.mutate };
}
