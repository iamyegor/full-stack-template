"use client";

import { Link } from "@/i18n/routing";
import { AlertCircle, Home, RefreshCcw } from "lucide-react";

interface ErrorComponentProps {
    error: Error & { digest?: string };
    reset: () => void;
}

export default function Error({ error, reset }: ErrorComponentProps) {
    return (
        <div className="bg-gradient-to-br from-purple-100 to-indigo-100 min-h-screen font-sans">
            <div className="container mx-auto min-h-screen flex justify-center items-center py-12">
                <div className="w-full max-w-md p-8 bg-white border border-purple-300 rounded-3xl duration-300 flex flex-col items-center">
                    <div className="flex flex-col items-center">
                        <AlertCircle className="w-24 h-24 text-purple-600 mb-3" />
                        <h1 className="text-[32px] lg:text-[45px] font-bold text-purple-800 mb-4 text-center leading-[1.1]">
                            Oops! Something went wrong
                        </h1>
                        <p className="text-purple-600 mb-12 text-center">
                            {error?.message || "An unexpected error occurred. Please try again."}
                            {error?.digest && (
                                <span className="block mt-2 text-sm text-purple-500">
                                    Error Digest: {error.digest}
                                </span>
                            )}
                        </p>
                    </div>

                    <div className="flex gap-4">
                        <button
                            onClick={reset}
                            className="flex items-center gap-2 bg-purple-600 hover:bg-purple-700 text-white font-bold py-3 px-6 rounded-full text-lg transition-colors duration-300"
                        >
                            <RefreshCcw className="w-5 h-5" />
                            Try Again
                        </button>

                        <Link
                            href="/"
                            className="flex items-center gap-2 bg-indigo-600 hover:bg-indigo-700 text-white font-bold py-3 px-6 rounded-full text-lg transition-colors duration-300"
                        >
                            <Home className="w-5 h-5" />
                            Go Home
                        </Link>
                    </div>
                </div>
            </div>
        </div>
    );
}
