"use client";

import Loader from "@/components/loading/Loader/Loader";
import { Button } from "@/components/ui/button";
import { Form, FormControl, FormField, FormItem, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import PasswordInput from "@/features/auth/components/PasswordInput";
import signInFormSchema from "@/features/auth/schemas/signInFormSchema";
import authApi from "@/lib/api/authApi";
import ServerErrorResponse from "@/types/errors/ServerErrorResponse";
import { zodResolver } from "@hookform/resolvers/zod";
import { AxiosError } from "axios";
import { Mail } from "lucide-react";
import Link from "next/link";
import { usePostHog } from "posthog-js/react";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";

type FormData = z.infer<typeof signInFormSchema>;

export default function SignInForm() {
    const [submitError, setSubmitError] = useState<string | null>(null);
    const posthog = usePostHog();

    const form = useForm<FormData>({
        resolver: zodResolver(signInFormSchema),
        defaultValues: {
            email: "",
            password: "",
        },
    });

    async function onSubmit(data: FormData) {
        try {
            setSubmitError(null);
            await authApi.post("/auth/sign-in", data);
            posthog.capture("user_sign_in");
        } catch (error) {
            const axiosError = error as AxiosError<ServerErrorResponse>;

            if (axiosError.response?.status === 429) {
                setSubmitError(
                    "You reached the maximum number of login attempts. Wait for 10 minutes before trying again."
                );
            } else if (axiosError.response?.data.errorCode) {
                if (axiosError.response.data.errorCode === "invalid.credentials") {
                    setSubmitError("The credentials you entered are incorrect.");
                }
            } else {
                setSubmitError("An unexpected error occurred. Please try again later.");
            }
        }
    }

    return (
        <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)}>
                <FormField
                    control={form.control}
                    name="email"
                    render={({ field }) => (
                        <FormItem className="mb-6">
                            <FormControl>
                                <div className="relative">
                                    <Mail className="absolute left-3 top-3 text-purple-600 w-6 h-6" />
                                    <Input
                                        className={`pl-12 ${
                                            form.formState.errors.email ? "!border-red-500" : ""
                                        }`}
                                        placeholder="Email"
                                        type="email"
                                        {...field}
                                    />
                                </div>
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    )}
                />

                <FormField
                    control={form.control}
                    name="password"
                    render={({ field }) => (
                        <FormItem className="mb-6">
                            <FormControl>
                                <div className="relative">
                                    <PasswordInput
                                        field={field}
                                        error={!!form.formState.errors.password}
                                    />
                                </div>
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    )}
                />

                <Link
                    href="reset-password"
                    className="text-sm text-purple-600 hover:text-purple-700 transition-colors flex space-x-1 mb-6"
                >
                    <span className="text-neutral-600">Don't remember your password?</span>{" "}
                    <span className="underline decoration-purple-500">Reset it</span>
                </Link>

                <Button
                    type="submit"
                    disabled={form.formState.isSubmitting}
                    variant="submit"
                    size="auto"
                >
                    {form.formState.isSubmitting ? <Loader /> : "Sign In"}
                </Button>

                {submitError && (
                    <p className="text-center text-sm text-red-500 mt-3">{submitError}</p>
                )}

                <p className="text-center text-neutral-600 text-sm mt-6">
                    Don't have an account?{" "}
                    <Link
                        href="signup"
                        className="text-purple-600 hover:text-purple-800 transition-colors font-medium"
                    >
                        Sign up
                    </Link>
                </p>
            </form>
        </Form>
    );
}
