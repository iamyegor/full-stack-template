import { useMediaQuery } from "react-responsive";

export default function useMediaQueries() {
    const isXsScreen = useMediaQuery({ minWidth: 420 });
    const isSmScreen = useMediaQuery({ minWidth: 640 });
    const isMdScreen = useMediaQuery({ minWidth: 768 });
    const isLgScreen = useMediaQuery({ minWidth: 1024 });
    const isXlScreen = useMediaQuery({ minWidth: 1280 });

    return {
        isXsScreen,
        isSmScreen,
        isMdScreen,
        isLgScreen,
        isXlScreen,
    };
}
