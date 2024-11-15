import { Input } from "@/components/ui/input";
import { Eye, EyeOff, KeyRound } from "lucide-react";
import { useState } from "react";

interface PasswordInputProps {
    field: any;
    error: boolean;
}

export default function PasswordInput({ field, error }: PasswordInputProps) {
    const [showPassword, setShowPassword] = useState(false);

    return (
        <div className="space-y-2">
            <div className="relative">
                <KeyRound className="absolute left-3 top-3 text-purple-600 w-6 h-6" />
                <Input
                    {...field}
                    type={showPassword ? "text" : "password"}
                    placeholder="Password"
                    className={`pl-12 ${error ? "!border-red-500" : ""}`}
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
        </div>
    );
}
