import { CircleX, Eye, EyeOff, KeyRound } from "lucide-react";
import { useState } from "react";
import { FieldErrors, Path, UseFormRegister } from "react-hook-form";

interface PasswordInputProps<T extends { password: string }> {
    register: UseFormRegister<T>;
    errors: FieldErrors<T>;
}

export default function PasswordInput<T extends { password: string }>({
    register,
    errors,
}: PasswordInputProps<T>) {
    const [showPassword, setShowPassword] = useState(false);

    return (
        <div className="space-y-2">
            <div className="relative">
                <KeyRound className="absolute left-3 top-3 text-purple-600 w-6 h-6" />
                <input
                    {...register("password" as Path<T>)}
                    type={showPassword ? "text" : "password"}
                    placeholder="Password"
                    className={`w-full pl-12 pr-12 py-3 rounded-xl border ${
                        errors.password ? "border-red-500" : "border-purple-300"
                    } focus:border-purple-500 focus:ring-2 focus:ring-purple-200 outline-none transition-all`}
                />
                <button
                    type="button"
                    onClick={() => setShowPassword(!showPassword)}
                    className="absolute right-3 top-3 text-purple-600 hover:text-purple-700 focus:outline-none"
                    aria-label={showPassword ? "Hide password" : "Show password"}
                >
                    {showPassword ? <Eye className="w-6 h-6" /> : <EyeOff className="w-6 h-6" />}
                </button>
            </div>
            {errors.password && (
                <div className="text-sm flex items-center ml-2 space-x-2 text-red-500">
                    <CircleX className="h-4 w-4" />
                    <p>{String(errors.password.message)}</p>
                </div>
            )}
        </div>
    );
}
