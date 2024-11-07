using Domain.DomainErrors;
using Domain.User.ValueObjects;
using Infrastructure.Auth.Authentication;
using Infrastructure.Auth.VkAuth;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using XResults;

namespace Application.User.Commands.SignInWithVk;

public record SignInWithVkCommand(string Code, string VkDeviceId, string? DeviceId)
    : IRequest<Result<VkAuthResult, Error>>;

public class SignInWithVkCommandHandler
    : IRequestHandler<SignInWithVkCommand, Result<VkAuthResult, Error>>
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

    public async Task<Result<VkAuthResult, Error>> Handle(
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

        Domain.User.User? userWithSameVkId = await _context.Users.FirstOrDefaultAsync(
            x => x.VkUserId == vkUserId,
            ct
        );

        using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(ct);

        Domain.User.User user;
        SocialAuthStatus authStatus;
        if (userWithSameVkId != null && userWithSameVkId.IsEmailVerified)
        {
            user = userWithSameVkId;
            authStatus = SocialAuthStatus.CurrentUser;
        }
        else
        {
            await _userRemover.RemoveUserIfExists(userWithSameVkId, ct);

            user = new Domain.User.User(vkUserId);
            _context.Users.Add(user);
            authStatus = SocialAuthStatus.NewUser;
        }

        Tokens tokens = _tokensGenerator.GenerateTokens(user);
        user.AddRefreshToken(new RefreshToken(tokens.RefreshToken, deviceId));

        await _context.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);

        return Result.Ok(new VkAuthResult(tokens, authStatus));
    }
}
