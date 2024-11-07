import fetchPagedTodos from "@/app/itodo/services/fetchPagedTodos";
import { useInfiniteQuery } from "@tanstack/react-query";
import { useEffect } from "react";
import { useInView } from "react-intersection-observer";

export default function usePagedTodos() {
    const { ref: todosEndRef, inView: listEndInView } = useInView();
    const { data, fetchNextPage, hasNextPage, isLoading, isError } = useInfiniteQuery({
        queryKey: ["todos-paged"],
        queryFn: ({ pageParam }) => fetchPagedTodos({ pageParam }),
        initialPageParam: 1,
        getNextPageParam: (lastPage) => {
            return lastPage.nextPage;
        },
    });

    const todos = data?.pages.flatMap((page) => page.todos) ?? [];

    useEffect(() => {
        if (listEndInView && hasNextPage) {
            fetchNextPage();
        }
    });

    return { todos, todosEndRef, hasNextPage, isLoading, isError };
}
