"use client";

import Checkbox from "@/components/CheckBox/CheckBox";
import Loader from "@/components/loading/Loader/Loader";
import { Button } from "@/components/ui/button";
import { Form, FormControl, FormField, FormItem, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import PasswordInput from "@/features/auth/components/PasswordInput";
import signUpFormSchema from "@/features/auth/schemas/signUpFormSchema";
import Link from "@/features/i18n/nextjsSpecific/Link";
import authApi from "@/lib/api/authApi";
import ServerErrorResponse from "@/types/errors/ServerErrorResponse";
import { zodResolver } from "@hookform/resolvers/zod";
import { AxiosError } from "axios";
import { Mail } from "lucide-react";
import { usePostHog } from "posthog-js/react";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import getSignUpError from "@/features/auth/utils/getSignUpError";

type SignUpFormValues = z.infer<typeof signUpFormSchema>;

export default function SignUpForm() {
    const posthog = usePostHog();
    const [submitError, setSubmitError] = useState<string | null>(null);

    const form = useForm<SignUpFormValues>({
        resolver: zodResolver(signUpFormSchema),
        defaultValues: {
            email: "",
            password: "",
            consent: false,
        },
    });

    async function onSubmit(data: SignUpFormValues) {
        try {
            setSubmitError(null);
            await authApi.post("/auth/sign-up", data);
            posthog.capture("user_sign_up");
        } catch (error) {
            setSubmitError(getSignUpError(error as AxiosError<ServerErrorResponse>));
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

                <FormField
                    control={form.control}
                    name="consent"
                    render={({ field }) => (
                        <FormItem className="mb-6">
                            <FormControl>
                                <div className="flex items-start space-x-2">
                                    <Checkbox
                                        id="consent"
                                        className={`mt-1 ${
                                            form.formState.errors.consent
                                                ? "!border-red-500"
                                                : "!border-purple-500"
                                        }`}
                                        onClick={() => {
                                            field.onChange(!field.value);
                                        }}
                                        isChecked={field.value}
                                        disabled={form.formState.isSubmitting}
                                    />
                                    <label
                                        htmlFor="consent"
                                        className={`text-sm ${
                                            form.formState.errors.consent
                                                ? "text-red-500"
                                                : "text-neutral-600"
                                        } cursor-pointer`}
                                    >
                                        I agree to the terms and conditions and privacy policy.
                                    </label>
                                </div>
                            </FormControl>
                        </FormItem>
                    )}
                />

                <Button
                    type="submit"
                    variant="submit"
                    size="auto"
                    disabled={form.formState.isSubmitting}
                >
                    {form.formState.isSubmitting ? <Loader /> : "Sign Up"}
                </Button>

                {submitError && (
                    <p className="text-center text-sm text-red-500 mt-4">{submitError}</p>
                )}

                <p className="text-center text-sm text-gray-600 mt-6">
                    Already have an account?{" "}
                    <Link
                        href="/signin"
                        className="text-purple-600 hover:text-purple-700 font-medium"
                    >
                        Sign In
                    </Link>
                </p>
            </form>
        </Form>
    );
}
