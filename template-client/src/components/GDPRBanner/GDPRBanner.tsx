"use client";

import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { Button } from "@/components/ui/button";
import { useEffect, useState } from "react";

const GDPR_CONSENT_KEY = "gdprConsent";

export default function GDPRBanner() {
    const [showBanner, setShowBanner] = useState(false);

    useEffect(() => {
        const consentStatus = localStorage.getItem(GDPR_CONSENT_KEY);
        if (consentStatus === null) {
            setShowBanner(true);
        }
    }, []);

    function onDismiss() {
        localStorage.setItem(GDPR_CONSENT_KEY, "true");
        setShowBanner(false);
    }

    if (!showBanner) return null;

    return (
        <div className="fixed bottom-0 left-0 right-0 p-4 bg-background border-t">
            <Alert className="max-w-4xl mx-auto">
                <AlertTitle className="text-lg font-semibold">Data Processing Notice</AlertTitle>
                <AlertDescription className="mt-2">
                    <p className="mb-4">
                        We use cookies and similar technologies to analyze site traffic and improve
                        your experience. This includes processing technical data for analytics and
                        security purposes. See our Privacy Policy for details.
                    </p>
                    <div className="flex">
                        <Button onClick={onDismiss}>I understand</Button>
                    </div>
                </AlertDescription>
            </Alert>
        </div>
    );
}
