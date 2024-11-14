"use client";

import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { useState } from "react";
import dynamic from "next/dynamic";
import { PHProvider } from "@/features/analytics/components/PHProvider";

const PostHogPageView = dynamic(() => import("../../features/analytics/components/PostHogPageView"), {
    ssr: false,
});

export default function Providers({ children }: { children: React.ReactNode }) {
    const [queryClient] = useState(
        () => new QueryClient({ defaultOptions: { queries: { retry: 1 } } })
    );

    return (
        <PHProvider>
            <QueryClientProvider client={queryClient}>
                <PostHogPageView />
                {children}
            </QueryClientProvider>
        </PHProvider>
    );
}
