using System.Text.RegularExpressions;
using Domain.Common;
using Domain.DomainErrors;
using XResults;

namespace Domain.User.ValueObjects;

public class Name : ValueObject
{
    private Name(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Name, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.Name.IsRequired;
        }

        string name = value.Trim();
        if (name.Length < 2)
        {
            return Errors.Name.MustBeAtLeastTwoCharacters;
        }

        if (Regex.IsMatch(name, "^[a-zA-Z]+$"))
        {
            return Errors.Name.MustContainOnlyLetters;
        }

        return new Name(name);
    }

    protected override IEnumerable<object?> GetPropertiesForComparison()
    {
        yield return Value;
    }
}
