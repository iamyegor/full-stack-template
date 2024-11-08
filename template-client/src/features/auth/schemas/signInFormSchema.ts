import { z } from "zod";

const signInFormSchema = z.object({
    email: z.string().email(),
    password: z
        .string()
        .min(8)
        .regex(/[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>/?0-9]/, {
            message: "Password must contain at least one number or special character",
        }),
    consent: z.boolean().refine((val) => val === true, {
        message: "You must agree to your terms of service & privacy policy",
    }),
});

export default signInFormSchema;
