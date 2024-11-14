"use client";

import { Button } from "@/components/ui/button";
import { CheckCircle, Clock } from "lucide-react";
import { Todo } from "../types/Todo";

interface TodoListProps {
    todos: Todo[];
    onToggleComplete: (todoId: number, completed: boolean) => void;
}

export default function TodoList({ todos, onToggleComplete }: TodoListProps) {
    return (
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
                            onClick={() => onToggleComplete(todo.id, !todo.completed)}
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
        </ul>
    );
}
