export default class EmptyResult {
    isSuccess: boolean;
    isFailure: boolean;
    errorMessage: string | null;

    private constructor(isSuccess: boolean, errorMessage: string | null) {
        this.isSuccess = isSuccess;
        this.isFailure = !isSuccess;
        this.errorMessage = errorMessage;
    }

    public static Ok(): EmptyResult {
        return new EmptyResult(true, null);
    }

    public static Fail(errorMessage: string): EmptyResult {
        return new EmptyResult(false, errorMessage);
    }
}
