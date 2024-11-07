import sendMarkTodoCompletedRequest from "@/app/ftodo/services/sendMarkTodoCompletedRequest";
import Todo from "@/src/types/Todo";
import { useMutation, useQueryClient } from "@tanstack/react-query";

const queryKey = ["todos"];

export default function useMarkTodoCompleted() {
    const queryClient = useQueryClient();

    const markTodoCompletedMutation = useMutation({
        mutationFn: sendMarkTodoCompletedRequest,
        onMutate: async (todoId: number) => {
            await queryClient.cancelQueries({ queryKey });

            const previousData = queryClient.getQueryData<Todo[]>(queryKey);

            queryClient.setQueryData<Todo[]>(queryKey, (data) => {
                if (!data) return previousData;

                return [
                    ...data.map((todo) =>
                        todo.id === todoId ? { ...todo, completed: true } : todo
                    ),
                ];
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
