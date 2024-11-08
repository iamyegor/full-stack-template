import React from "react";
import CheckSvg from "@/components/CheckBox/assets/check.svg";

interface CheckboxProps {
    id?: string;
    isChecked: boolean;
    onClick: () => void;
    className?: string;
    disabled?: boolean;
}

export default function Checkbox({ id, isChecked, onClick, className, disabled }: CheckboxProps) {
    return (
        <button
            id={id}
            className={`w-[18px] h-[18px] rounded flex justify-center items-center cursor-pointer border 
                             ${
                                 isChecked
                                     ? "bg-purple-500 border-purple-500"
                                     : "border-neutral-400"
                             } pl-[1px] pt-[1px] flex-shrink-0 ${className}`}
            onClick={onClick}
            type="button"
            disabled={disabled}
        >
            {isChecked && <CheckSvg className="w-full h-full" />}
        </button>
    );
}
