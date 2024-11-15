import { z } from "zod";

const signInFormSchema = z.object({
    email: z.string().email(),
    password: z
        .string()
        .min(8)
        .max(32)
        .regex(/^(?=.*[a-z])(?=.*[A-Z])(?=.*[\\d\\W]).*$/, {
            message:
                "Password must contain at least one uppercase letter, one lowercase letter and one number or special character",
        }),
});

export default signInFormSchema;
