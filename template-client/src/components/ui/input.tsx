import * as React from "react";

import { cn } from "@/lib/utils";

const Input = React.forwardRef<HTMLInputElement, React.ComponentProps<"input">>(
    ({ className, type, ...props }, ref) => {
        return (
            <input
                type={type}
                className={cn(
                    "px-4 py-3 !h-auto border border-purple-300 focus:border-purple-500 focus:ring-2 focus:ring-purple-200 transition-all flex w-full outline-none rounded-xl",
                    className
                )}
                ref={ref}
                {...props}
            />
        );
    }
);
Input.displayName = "Input";

export { Input };
