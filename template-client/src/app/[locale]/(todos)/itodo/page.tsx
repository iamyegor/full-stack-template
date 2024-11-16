"use client";

import TodoList from "@/features/todos/components/TodoList";
import TodoListLayout from "@/features/todos/components/TodoListLayout";
import usePagedTodos from "@/features/todos/hooks/useFetchTodosInfinitely";
import usePagedTodoChangeCompletion from "@/features/todos/hooks/usePagedTodoChangeCompletion";
import useAddPagedTodo from "@/features/todos/hooks/useAddPagedTodo";
import { Loader2 } from "lucide-react";
import AddTodoForm from "@/features/todos/components/AddTodoForm";
import { UseMutationResult } from "@tanstack/react-query";
import useErrorAlertMessage from "@/features/todos/hooks/useErrorAlertMessage";

export default function InfiniteTodoListPage() {
    const { errorMessage, setErrorMessage } = useErrorAlertMessage();
    const { todos, todosEndRef, hasNextPage, isLoading, isError } = usePagedTodos();
    const changeCompletionStatus = usePagedTodoChangeCompletion({ setErrorMessage });
    const addTodoMutation = useAddPagedTodo({ setErrorMessage });

    return (
        <TodoListLayout
            isLoading={isLoading}
            isError={isError}
            errorMessage={errorMessage}
            onErrorAlertClose={() => setErrorMessage(null)}
        >
            <AddTodoForm addTodoMutation={addTodoMutation as UseMutationResult} />
            <div className="flex flex-col w-full items-center">
                <TodoList
                    todos={todos}
                    onToggleComplete={(todoId, completed) =>
                        changeCompletionStatus.mutate({ todoId, completed })
                    }
                />

                <div ref={todosEndRef} />
            </div>

            {hasNextPage && (
                <div className="pb-10">
                    <Loader2 className="w-8 h-8 text-blue-600 animate-spin" />
                </div>
            )}
        </TodoListLayout>
    );
}
