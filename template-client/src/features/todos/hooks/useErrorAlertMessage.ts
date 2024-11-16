import { useState, useEffect, useRef } from "react";

export default function useErrorAlertMessage(duration: number = 5000) {
    const [errorMessage, setErrorMessage] = useState<string | null>(null);
    const errorTimeout = useRef<NodeJS.Timeout | null>(null);

    useEffect(() => {
        if (errorMessage) {
            if (errorTimeout.current) {
                clearTimeout(errorTimeout.current);
            }

            errorTimeout.current = setTimeout(() => {
                setErrorMessage(null);
            }, duration);
        }
    }, [errorMessage, duration]);

    return {
        errorMessage,
        setErrorMessage,
    };
} 