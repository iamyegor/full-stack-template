"use client";

import Link from "@/features/i18n/nextjsSpecific/Link";
import { LogIn } from "lucide-react";
import { usePostHog } from "posthog-js/react";

export default function GoToSignInButton() {
    const posthog = usePostHog();

    return (
        <Link
            href="/signin"
            className="absolute right-5 top-5 bg-blue-600 p-1 rounded-full w-11 h-11 flex items-center justify-center"
            onClick={() => posthog.capture("user_goes_to_signin_page")}
        >
            <LogIn className="text-white mr-1" />
        </Link>
    );
}
