import { Button } from "@/components/ui/button";
import { ArrowLeft } from "lucide-react";
import Link from "@/features/i18n/nextjsSpecific/Link";

export default function NotFound() {
    return (
        <div className="flex min-h-screen flex-col items-center justify-center bg-neutral-900 text-white p-6 font-sans">
            <h1 className="mb-4 text-[120px] leading-none tracking-tight font-bold">404</h1>
            <p className="mb-8 text-xl font-medium">What you looking for wasn't found</p>
            <Link href="/">
                <Button className="px-6 py-3 text-sm font-medium text-black bg-white rounded-md hover:bg-gray-200 transition-colors flex justify-center items-center space-x-2">
                    <ArrowLeft className="w-6 h-6" />
                    <span className="text-lg">Home</span>
                </Button>
            </Link>
        </div>
    );
}