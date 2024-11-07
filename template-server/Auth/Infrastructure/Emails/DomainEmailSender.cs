using System.Reflection;
using Domain.User.ValueObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Emails;

public class DomainEmailSender
{
    private readonly EmailSender _emailSender;

    private readonly IWebHostEnvironment _env;

    private readonly string _htmlFolderPath = Path.Combine(
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
        "Emails",
        "ui"
    );

    public DomainEmailSender(EmailSender emailSender, IWebHostEnvironment env)
    {
        _emailSender = emailSender;
        _env = env;
    }

    public async Task SendEmailVerificationCode(string email, int code)
    {
        string htmlContent = await File.ReadAllTextAsync(
            Path.Combine(_htmlFolderPath, "confirm-email.html")
        );

        string emailBody = htmlContent.Replace("{code}", code.ToString());
        await _emailSender.SendAsync("Код подтверждения NetIQ", emailBody, email);
    }

    public async Task SendPasswordReset(PasswordResetToken token, string email)
    {
        string htmlContent = await File.ReadAllTextAsync(
            Path.Combine(_htmlFolderPath, "password-reset.html")
        );

        string address = _env.IsDevelopment()
            ? "http://localhost:80"
            : Environment.GetEnvironmentVariable("SITE_URL")!;

        address += "/";

        string emailBody = htmlContent
            .Replace("{host}", address)
            .Replace("{token}", token.Value.ToString());
        await _emailSender.SendAsync("Сброс пароля NetIQ", emailBody, email);
    }
}
