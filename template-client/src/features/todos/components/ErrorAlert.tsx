import { AlertCircle, X } from "lucide-react";

interface ErrorAlertProps {
    message: string;
    onClose: () => void;
}

export default function ErrorAlert({ message, onClose }: ErrorAlertProps) {
    return (
        <div className="fixed top-4 left-1/2 -translate-x-1/2 bg-red-100 border border-red-400 text-red-700 px-6 py-3 rounded-full shadow-lg">
            <span>{message}</span>
            <button 
                onClick={onClose}
                className="absolute -top-2 -right-1 border border-red-400 hover:bg-red-200 rounded-full p-1 transition-colors bg-red-100"
            >
                <X className="w-4 h-4" />
            </button>
        </div>
    );
} 