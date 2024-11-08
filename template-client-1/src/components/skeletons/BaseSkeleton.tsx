import { ComponentProps } from "react";
import Skeleton from "react-loading-skeleton";

interface BaseSkeletonProps extends ComponentProps<typeof Skeleton> {}

export default function LoadingSkeleton({ className, ...rest }: BaseSkeletonProps) {
    return (
        <div className={className}>
            <Skeleton
                baseColor="#303030"
                highlightColor="#474747"
                height="100%"
                width="100%"
                borderRadius={12}
                duration={2}
                {...rest}
            />
        </div>
    );
}
