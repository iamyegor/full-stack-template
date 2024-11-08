using Domain.DomainErrors;
using Domain.Users.ValueObjects;
using Infrastructure.Data;
using Infrastructure.Features.Auth;
using Infrastructure.Features.Auth.Vk;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using XResults;

namespace Application.SignIn.Commands.SignInWithVk;

public record SignInWithVkCommand(string Code, string VkDeviceId, string? DeviceId)
    : IRequest<Result<SignInWithVkResult, Error>>;

public record SignInWithVkResult(Infrastructure.Features.Auth.Tokens Tokens, SocialAuthStatus AuthStatus);

public class SignInWithVkCommandHandler
    : IRequestHandler<SignInWithVkCommand, Result<SignInWithVkResult, Error>>
{
    private readonly ApplicationContext _context;
    private readonly TokensGenerator _tokensGenerator;
    private readonly UserRemover _userRemover;
    private readonly VkAuthTokenManager _vkAuthTokenManager;

    public SignInWithVkCommandHandler(
        VkAuthTokenManager vkAuthTokenManager,
        ApplicationContext context,
        TokensGenerator tokensGenerator,
        UserRemover userRemover
    )
    {
        _vkAuthTokenManager = vkAuthTokenManager;
        _context = context;
        _tokensGenerator = tokensGenerator;
        _userRemover = userRemover;
    }

    public async Task<Result<SignInWithVkResult, Error>> Handle(
        SignInWithVkCommand command,
        CancellationToken ct
    )
    {
        Result<string, Error> vkUserId = await _vkAuthTokenManager.GetVkUserId(
            command.Code,
            command.VkDeviceId
        );
        if (vkUserId.IsFailure)
        {
            return vkUserId.Error;
        }

        DeviceId deviceId = DeviceId.Create(command.DeviceId);

        Domain.Users.User? userWithSameVkId = await _context.Users.FirstOrDefaultAsync(
            x => x.VkUserId == vkUserId,
            ct
        );

        using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(ct);

        Domain.Users.User user;
        SocialAuthStatus authStatus;
        if (userWithSameVkId != null && userWithSameVkId.IsEmailVerified)
        {
            user = userWithSameVkId;
            authStatus = SocialAuthStatus.CurrentUser;
        }
        else
        {
            await _userRemover.RemoveUserIfExists(userWithSameVkId, ct);

            user = new Domain.Users.User(vkUserId);
            _context.Users.Add(user);
            authStatus = SocialAuthStatus.NewUser;
        }

        Infrastructure.Features.Auth.Tokens tokens = _tokensGenerator.GenerateTokens(user);
        user.AddRefreshToken(new RefreshToken(tokens.RefreshToken, deviceId));

        await _context.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);

        return Result.Ok(new SignInWithVkResult(tokens, authStatus));
    }
}
