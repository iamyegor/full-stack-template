export default class SuccessOr<TError> {
    isSuccess: boolean;
    isFailure: boolean;
    error: TError | null;

    private constructor(isSuccess: boolean, errorMessage: TError | null) {
        this.isSuccess = isSuccess;
        this.isFailure = !isSuccess;
        this.error = errorMessage;
    }

    public static Ok<TError>(): SuccessOr<TError> {
        return new SuccessOr<TError>(true, null);
    }

    public static Fail<TError>(errorMessage: TError): SuccessOr<TError> {
        return new SuccessOr<TError>(false, errorMessage);
    }
}
