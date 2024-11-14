import fetchTodos from "@/features/todos/api/fetchTodos";
import FiniteTodoList from "@/features/todos/components/FiniteTodoList";
import { HydrationBoundary, QueryClient, dehydrate } from "@tanstack/react-query";

export default async function FiniteTodoListPage() {
    const queryClient = new QueryClient();

    await queryClient.prefetchQuery({ queryKey: ["todos"], queryFn: fetchTodos });

    return (
        <HydrationBoundary state={dehydrate(queryClient)}>
            <FiniteTodoList />
        </HydrationBoundary>
    );
}
