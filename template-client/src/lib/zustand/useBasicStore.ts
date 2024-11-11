import { create } from "zustand";

interface BasicStore {
    info: string;
    setInfo: (info: string) => void;
}

const useBasicStore = create<BasicStore>()((set) => ({
    info: "",
    setInfo: (info: string) => set({ info }),
}));

export default useBasicStore;
