import Language from "@/types/Language";
import { create } from "zustand";

interface UiState {
    lang: Language;
    setLang: (lang: Language) => void;
}

const useUiStore = create<UiState>()((set) => ({
    lang: "en",
    setLang: (lang) => set({ lang }),
}));

export default useUiStore;
