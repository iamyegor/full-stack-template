import sendAddTodoRequest from "@/features/todos/api/sendAddTodoRequest";
import { Todo } from "@/features/todos/types/Todo";
import { useMutation, useQueryClient } from "@tanstack/react-query";

export default function useAddTodo() {
    const queryClient = useQueryClient();

    const mutation = useMutation({
        mutationFn: (text: string) => sendAddTodoRequest(text),
        onMutate: async (newTodoText) => {
            await queryClient.cancelQueries({ queryKey: ['todos'] });

            const previousTodos = queryClient.getQueryData<Todo[]>(['todos']);

            const optimisticTodo: Todo = {
                id: Date.now(),
                title: newTodoText,
                completed: false,
                createdAt: new Date().toISOString()
            };

            queryClient.setQueryData<Todo[]>(['todos'], (old = []) => [optimisticTodo, ...old]);

            return { previousTodos};
        },
        onError: (_, __, context) => {
            queryClient.setQueryData(['todos'], context?.previousTodos);
        },
        onSettled: () => {
            queryClient.invalidateQueries({ queryKey: ['todos'] });
        },
    });

    return { addTodo: mutation };
} 