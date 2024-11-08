import { useEffect } from "react";

declare global {
    namespace JSX {
        interface IntrinsicElements {
            "l-spiral": React.DetailedHTMLProps<React.HTMLAttributes<HTMLElement>, HTMLElement>;
        }
    }
}

export default function Loader() {
    useEffect(() => {
        async function getLoader() {
            const { spiral } = await import("ldrs");
            spiral.register();
        }
        getLoader();
    }, []);
    return <l-spiral color="coral"></l-spiral>;
}
