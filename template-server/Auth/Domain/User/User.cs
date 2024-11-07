using Domain.Common;
using Domain.User.ValueObjects;

namespace Domain.User;

public class User : AggregateRoot<UserId>
{
    private readonly List<RefreshToken> _refreshTokens = [];

    protected User()
        : base(new UserId()) { }

    public User(string vkUserId)
        : base(new UserId())
    {
        VkUserId = vkUserId;
    }

    public User(Email email, Password password)
        : base(new UserId())
    {
        Email = email;
        Password = password;
        IsEmailVerified = false;
    }

    public Email? Email { get; private set; }
    public Password? Password { get; private set; }

    public EmailVerificationCode? EmailVerificationCode { get; private set; } =
        EmailVerificationCode.CreateRandom();

    public IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens;
    public bool IsEmailVerified { get; private set; }
    public PasswordResetToken? PasswordResetToken { get; private set; }
    public string? VkUserId { get; }

    public void AddRefreshToken(RefreshToken refreshToken)
    {
        _refreshTokens.RemoveAll(rt => rt.DeviceId == refreshToken.DeviceId);
        _refreshTokens.Add(refreshToken);
    }

    public bool IsRefreshTokenExpired(DeviceId deviceId)
    {
        return RefreshTokens.First(x => x.DeviceId == deviceId).IsExpired;
    }

    public void ConfirmEmail()
    {
        IsEmailVerified = true;
        EmailVerificationCode = null;
    }

    public void NewVerificationCode()
    {
        EmailVerificationCode = EmailVerificationCode.CreateRandom();
    }

    public void GeneratePasswordResetToken()
    {
        PasswordResetToken = PasswordResetToken.GenerateRandom();
    }

    public void ResetPassword(Password newPassword)
    {
        Password = newPassword;
        PasswordResetToken = null;
    }

    public void AddEmail(Email email)
    {
        Email = email;
    }

    public void ResetEmail()
    {
        Email = null;
    }
}
