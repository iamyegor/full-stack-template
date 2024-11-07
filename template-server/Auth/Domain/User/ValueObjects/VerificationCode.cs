using Domain.Common;
using Domain.DomainErrors;
using XResults;

namespace Domain.User.ValueObjects;

public class EmailVerificationCode : ValueObject
{
    protected EmailVerificationCode() { }

    private EmailVerificationCode(int value)
    {
        Value = value;
        ExpiryTime = DateTimeOffset.UtcNow.AddDays(1);
    }

    public int Value { get; }
    public DateTimeOffset ExpiryTime { get; }
    public bool IsExpired => DateTimeOffset.UtcNow > ExpiryTime;

    protected override IEnumerable<object?> GetPropertiesForComparison()
    {
        yield return Value;
    }

    public static EmailVerificationCode CreateRandom()
    {
        return new EmailVerificationCode(GetRandom5DigitNumber());
    }

    private static int GetRandom5DigitNumber()
    {
        Random random = new Random();
        return random.Next(10000, 100000);
    }

    public static Result<EmailVerificationCode, Error> Create(string code)
    {
        if (code.Length != 5)
        {
            return Errors.EmailVerificationCode.HasInvalidLength;
        }

        if (!int.TryParse(code, out int value))
        {
            return Errors.EmailVerificationCode.HasInvalidFormat;
        }

        return new EmailVerificationCode(value);
    }
}
