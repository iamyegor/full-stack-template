import { z } from "zod";

const signUpFormSchema = z.object({
    email: z.string().email("Please enter a valid email address"),
    password: z
        .string()
        .min(8)
        .max(32)
        .regex(/^(?=.*[a-z])(?=.*[A-Z])(?=.*[\\d\\W]).*$/, {
            message:
                "Password must contain at least one uppercase letter, one lowercase letter and one number or special character",
        }),
    consent: z.boolean().refine((val) => val === true, {
        message: "You must agree to the terms and conditions",
    }),
});

export default signUpFormSchema;
