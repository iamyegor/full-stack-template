"use client";

import { Button } from "@/components/ui/button";
import { UseMutationResult } from "@tanstack/react-query";
import { Loader2 } from "lucide-react";
import { useState } from "react";

export default function AddTodoForm({ addTodoMutation }: { addTodoMutation: UseMutationResult }) {
    const [newTodoText, setNewTodoText] = useState("");

    function handleAddTodo(e: React.FormEvent) {
        e.preventDefault();

        if (!newTodoText.trim()) return;
        setNewTodoText("");

        addTodoMutation.mutate(newTodoText);
    }

    return (
        <form onSubmit={handleAddTodo} className="mb-4 flex gap-2 w-full max-w-md h-[50px]">
            <input
                type="text"
                value={newTodoText}
                onChange={(e) => setNewTodoText(e.target.value)}
                placeholder="Add a new todo..."
                className="flex-1 px-4 border border-gray-300 rounded-md outline-none"
                disabled={addTodoMutation.isPending}
            />
            <Button
                type="submit"
                disabled={addTodoMutation.isPending}
                className="bg-blue-500 text-white rounded-md hover:bg-blue-600 !h-auto !px-4"
            >
                {addTodoMutation.isPending ? (
                    <Loader2 className="w-4 h-4 animate-spin" />
                ) : (
                    "Add Todo"
                )}
            </Button>
        </form>
    );
}
