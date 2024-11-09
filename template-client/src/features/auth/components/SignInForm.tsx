"use client";

import Checkbox from "@/components/CheckBox/CheckBox";
import Loader from "@/components/loading/Loader/Loader";
import PasswordInput from "@/features/auth/components/PasswordInput";
import signInFormSchema from "@/features/auth/schemas/signInFormSchema";
import authApi from "@/lib/api/authApi";
import ServerErrorResponse from "@/types/errors/ServerErrorResponse";
import { zodResolver } from "@hookform/resolvers/zod";
import { AxiosError } from "axios";
import { CircleX, Mail } from "lucide-react";
import Link from "next/link";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";

type FormData = z.infer<typeof signInFormSchema>;

export default function SignInForm() {
    const [submitError, setSubmitError] = useState<string | null>(null);

    const form = useForm<FormData>({
        resolver: zodResolver(signInFormSchema),
        defaultValues: {
            email: "",
            password: "",
            consent: false,
        },
    });

    const {
        register,
        handleSubmit,
        formState: { errors, isSubmitting },
        watch,
    } = form;

    const consent = watch("consent");

    async function onSubmit(data: FormData) {
        try {
            setSubmitError(null);
            await authApi.post("auth/sign-in", data);
        } catch (error) {
            const axiosError = error as AxiosError<ServerErrorResponse>;

            if (axiosError.response?.status === 429) {
                setSubmitError(
                    "You reached the maximum number of login attempts. Wait for 10 minutes before trying again."
                );
                return;
            }

            if (axiosError.response?.data.errorCode) {
                if (axiosError.response.data.errorCode === "invalid.credentials") {
                    setSubmitError("The credentials you entered are incorrect.");
                }
                return;
            }
            setSubmitError("An unexpected error occurred. Please try again later.");
        }
    }

    return (
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
            <div className="space-y-2">
                <div className="relative">
                    <Mail className="absolute left-3 top-3 text-purple-600 w-6 h-6" />
                    <input
                        {...register("email")}
                        type="text"
                        placeholder="Email"
                        className={`w-full pl-12 pr-4 py-3 rounded-xl border ${
                            errors.email ? "border-red-500" : "border-purple-300"
                        } focus:border-purple-500 focus:ring-2 focus:ring-purple-200 outline-none transition-all`}
                    />
                </div>
                {errors.email && (
                    <div className="text-sm flex items-center ml-2 space-x-2 text-red-500">
                        <CircleX className="h-4 w-4" />
                        <p>{errors.email.message}</p>
                    </div>
                )}
            </div>

            <PasswordInput register={register} errors={errors} />

            <div className="flex flex-col space-y-5">
                <Link
                    href="reset-password"
                    className="ml-1 text-sm text-purple-600 hover:text-purple-700 transition-colors flex space-x-1"
                >
                    <span className="text-purple-800">Don't remember your password?</span>
                    <span className="underline decoration-purple-500">Reset it</span>
                </Link>
                <div className="flex items-start space-x-2">
                    <input
                        type="checkbox"
                        {...register("consent")}
                        id="consent"
                        className="hidden"
                    />
                    <Checkbox
                        className={`mt-1 ${
                            errors.consent ? "!border-red-500" : "!border-purple-500"
                        }`}
                        id="consent-visual"
                        isChecked={consent}
                        onClick={() => {
                            const checkbox = document.getElementById("consent") as HTMLInputElement;
                            checkbox.click();
                        }}
                        disabled={isSubmitting}
                    />
                    <label
                        htmlFor="consent-visual"
                        className={`text-sm ${
                            errors.consent ? "text-red-500" : "text-purple-800"
                        } cursor-pointer`}
                    >
                        I agree to the terms and conditions and privacy policy.
                    </label>
                </div>
            </div>

            <button
                type="submit"
                disabled={isSubmitting}
                className="w-full bg-purple-600 hover:bg-purple-700 text-white font-bold py-3 px-6 rounded-full text-lg transition-colors duration-300 disabled:opacity-50 disabled:cursor-not-allowed flex justify-center"
            >
                {isSubmitting ? <Loader /> : "Sign In"}
            </button>

            {submitError && (
                <div className="text-sm w-full flex justify-center  ml-2 space-x-2 text-red-500 text-center">
                    <p>{submitError}</p>
                </div>
            )}

            <p className="text-center text-purple-800 text-sm">
                Don't have an account?{" "}
                <Link
                    href="signup"
                    className="text-purple-600 hover:text-purple-800 font-semibold transition-colors"
                >
                    Sign up
                </Link>
            </p>
        </form>
    );
}
