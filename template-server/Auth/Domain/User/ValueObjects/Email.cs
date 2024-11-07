using System.Text.RegularExpressions;
using Domain.Common;
using Domain.DomainErrors;
using XResults;

namespace Domain.User.ValueObjects;

public class Email : ValueObject
{
    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Email, Error> Create(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return Errors.Email.IsRequired;
        }

        string email = input.Trim().ToLower();
        if (email.Length > 150)
        {
            return Errors.Email.IsTooLong;
        }

        if (!Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$"))
        {
            return Errors.Email.HasInvalidSignature;
        }

        return new Email(email);
    }

    protected override IEnumerable<object?> GetPropertiesForComparison()
    {
        yield return Value;
    }
}
