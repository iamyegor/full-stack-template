using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Domain.Common;
using Domain.DomainErrors;
using XResults;

namespace Domain.User.ValueObjects;

public class Password : ValueObject
{
    private Password(string hashedPassword)
    {
        HashedPassword = hashedPassword;
    }

    public string HashedPassword { get; }

    public static Result<Password, Error> Create(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return Errors.Password.IsRequired;
        }

        string password = input.Trim();
        if (password.Length < 6 || password.Length > 50)
        {
            return Errors.Password.HasInvalidLength;
        }

        if (!Regex.IsMatch(password, "^(?=.*[a-z])(?=.*[A-Z])(?=.*[\\d\\W]).*$"))
        {
            return Errors.Password.HasInvalidSignature;
        }

        string hashedPassword = Convert.ToHexString(
            SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(password))
        );

        return new Password(hashedPassword);
    }

    protected override IEnumerable<object?> GetPropertiesForComparison()
    {
        yield return HashedPassword;
    }

    public bool Matches(string password)
    {
        string hashedIncomingPassword = Convert.ToHexString(
            SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(password))
        );

        return HashedPassword == hashedIncomingPassword;
    }
}
