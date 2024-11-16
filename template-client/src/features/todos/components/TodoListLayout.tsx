"use client";

import { Button } from "@/components/ui/button";
import Link from "@/features/i18n/nextjsSpecific/Link";
import ErrorAlert from "@/features/todos/components/ErrorAlert";
import { ArrowLeft, Loader2 } from "lucide-react";
import { ReactNode } from "react";

interface TodoListLayoutProps {
    isLoading?: boolean;
    isError?: boolean;
    children: ReactNode;
    errorMessage: string | null;
    onErrorAlertClose: () => void;
}

export default function TodoListLayout({
    isLoading,
    isError,
    children,
    errorMessage,
    onErrorAlertClose,
}: TodoListLayoutProps) {
    if (isLoading) {
        return (
            <div className="flex items-center justify-center h-screen bg-blue-50">
                <Loader2 className="w-12 h-12 text-blue-600 animate-spin" />
            </div>
        );
    }

    if (isError) {
        return (
            <div className="flex items-center justify-center h-screen bg-blue-50">
                <div className="text-red-600 font-semibold text-xl">Error fetching todos</div>
            </div>
        );
    }

    return (
        <div className="w-full min-h-screen bg-blue-50 flex flex-col items-center pt-8 px-4">
            {errorMessage && (
                <ErrorAlert message={errorMessage} onClose={onErrorAlertClose} />
            )}
            <Link href="/" className="absolute top-5 left-5">
                <Button variant="outline" size="icon" className="rounded-full hover:bg-blue-100">
                    <ArrowLeft className="w-5 h-5 text-blue-600" />
                </Button>
            </Link>

            <h1 className="mb-10 text-4xl md:text-5xl font-bold text-blue-800">My Todos</h1>

            {children}
        </div>
    );
}
