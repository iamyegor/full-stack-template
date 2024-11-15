import SignUpForm from "@/features/auth/components/SignUpForm";
import { UserPlus } from "lucide-react";

export default function SignUpPage() {
    return (
        <div className="bg-gradient-to-br from-purple-100 to-indigo-100 font-sans">
            <div className="container min-h-screen flex justify-center items-center py-12">
                <div className="w-full max-w-md p-8 bg-white border border-purple-300 rounded-3xl duration-300">
                    <div className="flex flex-col items-center mb-4">
                        <UserPlus className="w-20 h-20 text-purple-600 mb-3 -mr-3" />
                        <h1 className="text-[32px] lg:text-[45px] font-bold text-purple-800 mb-4 text-center leading-[1.1]">
                            Sign Up
                        </h1>
                        <p className="text-purple-600 mb-8 text-center">
                            Create an account to get started
                        </p>
                    </div>

                    <SignUpForm />
                </div>
            </div>
        </div>
    );
} 