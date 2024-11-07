import FiniteTodoList from "@/app/ftodo/components/FiniteTodoList/FiniteTodoList";
import fetchTodos from "@/app/ftodo/services/fetchTodos";
import { HydrationBoundary, QueryClient, dehydrate } from "@tanstack/react-query";

export default async function FiniteTodoListPage() {
    const queryClient = new QueryClient();

    await queryClient.prefetchQuery({ queryKey: ["todos"], queryFn: fetchTodos });

    return (
        <HydrationBoundary state={dehydrate(queryClient)}>
            <FiniteTodoList />;
        </HydrationBoundary>
    );
}
