using Domain.User;
using Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Cofigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users").HasKey(u => u.Id);
        builder
            .Property(u => u.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, guid => new UserId(guid))
            .ValueGeneratedNever();

        builder.OwnsOne(
            u => u.Email,
            email =>
            {
                email.Property(e => e.Value).HasColumnName("email");
                email.HasIndex(e => e.Value).IsUnique();
            }
        );
        builder.OwnsOne(
            u => u.Password,
            password =>
            {
                password.Property(p => p.HashedPassword).HasColumnName("hashed_password");
            }
        );
        builder.Property(u => u.IsEmailVerified).HasColumnName("is_email_verified");
        builder.OwnsMany(
            u => u.RefreshTokens,
            rt =>
            {
                rt.Property<int>("id");
                rt.HasKey("id");
                rt.ToTable("user_refresh_tokens");
                rt.Property(r => r.Value).HasColumnName("value");
                rt.OwnsOne(
                    r => r.DeviceId,
                    deviceId =>
                    {
                        deviceId.Property(d => d.Value).HasColumnName("device_id");
                    }
                );
                rt.Property(r => r.ExpiryTime).HasColumnName("expiry_time");

                rt.WithOwner().HasForeignKey("user_id");
                rt.Property("user_id").IsRequired();
            }
        );
        builder.OwnsOne(
            x => x.EmailVerificationCode,
            codeBuilder =>
            {
                codeBuilder.Property(x => x.Value).HasColumnName("email_verification_code");
                codeBuilder
                    .Property(x => x.ExpiryTime)
                    .HasColumnName("email_verification_code_expiry_time");
            }
        );
        builder.OwnsOne(
            x => x.PasswordResetToken,
            tokenBuilder =>
            {
                tokenBuilder.Property(x => x.Value).HasColumnName("password_reset_token");
                tokenBuilder
                    .Property(x => x.ExpiryTime)
                    .HasColumnName("password_reset_token_expiry_time");
            }
        );
        builder.Property(x => x.VkUserId).HasColumnName("vk_user_id");
    }
}
