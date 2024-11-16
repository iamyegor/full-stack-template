"use client";

import fetchTodos from "@/features/todos/api/fetchTodos";
import useAddTodo from "@/features/todos/hooks/useAddTodo";
import useTodoChangeCompletion from "@/features/todos/hooks/useTodoChangeCompletion";
import { UseMutationResult, useQuery } from "@tanstack/react-query";
import useErrorAlertMessage from "../hooks/useErrorAlertMessage";
import AddTodoForm from "./AddTodoForm";
import TodoList from "./TodoList";
import TodoListLayout from "./TodoListLayout";

export default function FiniteTodoList() {
    const { errorMessage, setErrorMessage } = useErrorAlertMessage();
    const { data, isLoading, isError } = useQuery({
        queryKey: ["todos"],
        queryFn: fetchTodos,
    });

    const { changeCompletionStatus } = useTodoChangeCompletion({ setErrorMessage });
    const { addTodo } = useAddTodo({ setErrorMessage });
    const todos = data ?? [];

    return (
        <TodoListLayout
            isLoading={isLoading}
            isError={isError}
            errorMessage={errorMessage}
            onErrorAlertClose={() => setErrorMessage(null)}
        >
            <AddTodoForm addTodoMutation={addTodo as UseMutationResult} />
            <TodoList
                todos={todos}
                onToggleComplete={(todoId, completed) =>
                    changeCompletionStatus({ todoId, completed })
                }
            />
        </TodoListLayout>
    );
}
