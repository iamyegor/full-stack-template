import LoaderSvg from "@/components/loading/Loader/assets/loader.svg";

export default function Loader({ className }: { className?: string }) {
    return <LoaderSvg className={`fill-white !w-8 !h-8 ${className}`} />;
}
