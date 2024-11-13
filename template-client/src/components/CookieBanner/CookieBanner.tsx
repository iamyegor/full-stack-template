"use client";

import { useState, useEffect } from "react";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { Button } from "@/components/ui/button";

const COOKIE_CONSENT_KEY = "cookieConsent";
const GA_MEASUREMENT_ID = "G-SDHY5BQ9YH";

declare global {
    interface Window {
        gtag: (...args: any[]) => void;
    }
}

function initGA(consent: boolean) {
    if (consent) {
        const script1 = document.createElement("script");
        script1.async = true;
        script1.src = `https://www.googletagmanager.com/gtag/js?id=${GA_MEASUREMENT_ID}`;

        const script2 = document.createElement("script");
        script2.innerHTML = `
      window.dataLayer = window.dataLayer || [];
      function gtag(){dataLayer.push(arguments);}
      gtag('js', new Date());
      gtag('config', '${GA_MEASUREMENT_ID}', {
        page_path: window.location.pathname,
      });
    `;

        document.head.appendChild(script1);
        document.head.appendChild(script2);
    }
}

export default function CookieBanner() {
    const [showBanner, setShowBanner] = useState(false);

    useEffect(() => {
        // Check if user has already made a choice
        const consentStatus = localStorage.getItem(COOKIE_CONSENT_KEY);
        if (consentStatus === null) {
            setShowBanner(true);
        } else {
            // Initialize GA if consent was previously given
            if (consentStatus === "true") {
                initGA(true);
            }
        }
    }, []);

    const handleConsent = (consent: boolean) => {
        localStorage.setItem(COOKIE_CONSENT_KEY, consent.toString());
        setShowBanner(false);
        initGA(consent);
    };

    if (!showBanner) return null;

    return (
        <div className="fixed bottom-0 left-0 right-0 p-4 bg-background border-t">
            <Alert className="max-w-4xl mx-auto">
                <AlertTitle className="text-lg font-semibold">Cookie Consent</AlertTitle>
                <AlertDescription className="mt-2">
                    <p className="mb-4">
                        We use cookies to analyze our website traffic and improve your browsing
                        experience. By clicking "Accept", you consent to our use of cookies for
                        analytics purposes.
                    </p>
                    <div className="flex gap-4">
                        <Button onClick={() => handleConsent(true)}>Accept</Button>
                        <Button onClick={() => handleConsent(false)} variant="outline">
                            Decline
                        </Button>
                    </div>
                </AlertDescription>
            </Alert>
        </div>
    );
};
