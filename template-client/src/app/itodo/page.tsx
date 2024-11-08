"use client";

import { Button } from "@/components/ui/button";
import usePagedTodos from "@/features/todos/hooks/useFetchTodosInfinitely";
import useMarkPagedTodoCompleted from "@/features/todos/hooks/useMarkPagedTodoCompleted";
import { ArrowLeft, CheckCircle, Clock, Loader2 } from "lucide-react";
import Link from "next/link";

export default function InfiniteTodoListPage() {
    const { todos, todosEndRef, hasNextPage, isLoading, isError } = usePagedTodos();
    const markTodoCompleted = useMarkPagedTodoCompleted();

    if (isLoading)
        return (
            <div className="flex items-center justify-center h-screen bg-blue-50">
                <Loader2 className="w-12 h-12 text-blue-600 animate-spin" />
            </div>
        );

    if (isError)
        return (
            <div className="flex items-center justify-center h-screen bg-blue-50">
                <div className="text-red-600 font-semibold text-xl">Error fetching todos</div>
            </div>
        );

    return (
        <div className="w-full min-h-screen bg-blue-50 flex flex-col items-center pt-8 px-4">
            <Link href="/" className="absolute top-5 left-5">
                <Button variant="outline" size="icon" className="rounded-full hover:bg-blue-100">
                    <ArrowLeft className="w-5 h-5 text-blue-600" />
                </Button>
            </Link>

            <h1 className="mb-10 text-4xl md:text-5xl font-bold text-blue-800">My Todos</h1>

            <ul className="w-full max-w-md flex flex-col items-center mb-8 space-y-4">
                {todos.map((todo, index) => (
                    <li key={index} className="w-full bg-white rounded-lg overflow-hidden">
                        <div className="p-4 flex justify-between items-center">
                            <p
                                className={`text-lg ${
                                    todo.completed ? "text-gray-500 line-through" : "text-gray-800"
                                }`}
                            >
                                {todo.title}
                            </p>
                            <Button
                                onClick={() => markTodoCompleted(todo.id)}
                                size="icon"
                                variant={todo.completed ? "ghost" : "outline"}
                                className={`rounded-full ${
                                    todo.completed
                                        ? "text-green-500 hover:text-green-600"
                                        : "text-blue-500 hover:text-blue-600"
                                }`}
                            >
                                {todo.completed ? (
                                    <CheckCircle className="w-5 h-5" />
                                ) : (
                                    <Clock className="w-5 h-5" />
                                )}
                            </Button>
                        </div>
                    </li>
                ))}
                <div ref={todosEndRef} />
            </ul>

            {hasNextPage && (
                <div className="pb-10">
                    <Loader2 className="w-8 h-8 text-blue-600 animate-spin" />
                </div>
            )}
        </div>
    );
}
