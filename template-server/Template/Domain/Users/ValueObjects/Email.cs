using System.Text.RegularExpressions;
using Domain.Common;
using Domain.Errors;
using Domain.Users.Errors;
using XResults;

namespace Domain.Users.ValueObjects;

public class Email : ValueObject
{
    protected Email() { }

    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Email, Error> Create(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return ErrorsEmail.IsRequired;

        string email = input.Trim().ToLower();
        if (email.Length > 150)
            return ErrorsEmail.IsTooLong;

        if (!Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$"))
            return ErrorsEmail.HasInvalidSignature;

        return new Email(email);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
