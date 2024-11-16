namespace Domain.Common.Errors;

public static class ErrorsConcurrency
{
    public static Error Occured => new("concurrency.error");
}
