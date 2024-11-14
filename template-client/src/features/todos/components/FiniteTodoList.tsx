"use client";

import fetchTodos from "@/features/todos/api/fetchTodos";
import useTodoChangeCompletion from "@/features/todos/hooks/useTodoChangeCompletion";
import { UseMutationResult, useQuery } from "@tanstack/react-query";
import TodoList from "./TodoList";
import TodoListLayout from "./TodoListLayout";
import AddTodoForm from "./AddTodoForm";
import useAddTodo from "@/features/todos/hooks/useAddTodo";

export default function FiniteTodoList() {
    const { data, isLoading, isError } = useQuery({
        queryKey: ["todos"],
        queryFn: fetchTodos,
    });

    const { changeCompletionStatus } = useTodoChangeCompletion();
    const { addTodo } = useAddTodo();
    const todos = data ?? [];

    return (
        <TodoListLayout isLoading={isLoading} isError={isError}>
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
