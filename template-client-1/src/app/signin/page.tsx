import React from "react";
import { LogIn, KeyRound, Mail, AlertCircle } from "lucide-react";
import { z } from "zod";
import { Form, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Alert, AlertDescription } from "@/components/ui/alert";

const formSchema = z.object({
    email: z.string().email(),
    password: z
        .string()
        .min(8)
        .regex(/[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>/?0-9]/), // at least one special character or digit
});

const SignInPage = () => {
    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            email: "",
            password: "",
        },
    });

    const {
        formState: { errors, isSubmitting },
    } = form;

    async function handleSubmit(data: z.infer<typeof formSchema>) {
        try {
            console.log(data);
            // Add your submission logic here
        } catch (error) {
            console.error("Submission error:", error);
        }
    }

    return (
        <div className="bg-gradient-to-br from-purple-100 to-indigo-100 font-sans">
            <button className="absolute right-5 top-5 bg-blue-600 p-1 rounded-full w-11 h-11 flex items-center justify-center">
                <LogIn className="text-white mr-1" />
            </button>

            <div className="container min-h-screen flex justify-center items-center py-12">
                <div className="w-full max-w-md p-8 bg-white border border-purple-300 rounded-3xl duration-300">
                    <div className="flex flex-col items-center mb-8">
                        <LogIn className="w-24 h-24 text-purple-600 mb-3" />
                        <h1 className="text-[32px] lg:text-[45px] font-bold text-purple-800 mb-4 text-center leading-[1.1]">
                            Sign In
                        </h1>
                        <p className="text-purple-600 mb-8 text-center">
                            Welcome back! Please sign in to continue
                        </p>
                    </div>

                    <Form {...form}>
                        <form onSubmit={form.handleSubmit(handleSubmit)} className="space-y-6">
                            <div className="space-y-2">
                                <div className="relative">
                                    <Mail className="absolute left-3 top-3 text-purple-600 w-6 h-6" />
                                    <input
                                        {...form.register("email")}
                                        type="email"
                                        placeholder="Email"
                                        className={`w-full pl-12 pr-4 py-3 rounded-xl border ${
                                            errors.email ? "border-red-500" : "border-purple-300"
                                        } focus:border-purple-500 focus:ring-2 focus:ring-purple-200 outline-none transition-all`}
                                    />
                                </div>
                                {errors.email && (
                                    <Alert variant="destructive" className="py-2 text-sm">
                                        <AlertCircle className="h-4 w-4" />
                                        <AlertDescription>{errors.email.message}</AlertDescription>
                                    </Alert>
                                )}
                            </div>

                            <div className="space-y-2">
                                <div className="relative">
                                    <KeyRound className="absolute left-3 top-3 text-purple-600 w-6 h-6" />
                                    <input
                                        {...form.register("password")}
                                        type="password"
                                        placeholder="Password"
                                        className={`w-full pl-12 pr-4 py-3 rounded-xl border ${
                                            errors.password ? "border-red-500" : "border-purple-300"
                                        } focus:border-purple-500 focus:ring-2 focus:ring-purple-200 outline-none transition-all`}
                                    />
                                </div>
                                {errors.password && (
                                    <Alert variant="destructive" className="py-2 text-sm">
                                        <AlertCircle className="h-4 w-4" />
                                        <AlertDescription>
                                            {errors.password.message}
                                        </AlertDescription>
                                    </Alert>
                                )}
                            </div>

                            <div className="flex items-center justify-between text-sm">
                                <label className="flex items-center">
                                    <input
                                        type="checkbox"
                                        className="rounded text-purple-600 focus:ring-purple-500 mr-2"
                                    />
                                    <span className="text-purple-800">Remember me</span>
                                </label>
                                <a
                                    href="#"
                                    className="text-purple-600 hover:text-purple-800 transition-colors"
                                >
                                    Forgot password?
                                </a>
                            </div>

                            <button
                                type="submit"
                                disabled={isSubmitting}
                                className="w-full bg-purple-600 hover:bg-purple-700 text-white font-bold py-3 px-6 rounded-full text-lg transition-colors duration-300 disabled:opacity-50 disabled:cursor-not-allowed"
                            >
                                {isSubmitting ? "Signing in..." : "Sign In"}
                            </button>

                            <p className="text-center text-purple-800">
                                Don't have an account?{" "}
                                <a
                                    href="#"
                                    className="text-purple-600 hover:text-purple-800 font-semibold transition-colors"
                                >
                                    Sign up
                                </a>
                            </p>
                        </form>
                    </Form>
                </div>
            </div>
        </div>
    );
};

export default SignInPage;
