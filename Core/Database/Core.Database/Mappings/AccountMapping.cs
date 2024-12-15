using Core.Database.Constants;
using Core.Database.Models.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Database.Mappings;

public class AccountMapping : IEntityTypeConfiguration<AccountModel>
{
    public void Configure(EntityTypeBuilder<AccountModel> builder)
    {
        builder.ToTable("Account");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Email)
            .IsUnique();
        builder.HasIndex(x => x.Username)
            .IsUnique();
        
        builder.Property(x => x.Username)
            .IsRequired(true)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(CharactersLength.MaxUsernameLength);
        builder.Property(x => x.Password)
            .IsRequired(true)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(CharactersLength.MaxEncryptedPasswordLength);
        builder.Property(x => x.Email)
            .IsRequired(true)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(CharactersLength.MaxEmailLength);
        builder.Property(x => x.BirthDate)
            .IsRequired(true)
            .HasConversion(
            v => v.ToDateTime(TimeOnly.MinValue), 
            v => DateOnly.FromDateTime(v));
        builder.Property(x => x.CreatedAt)
            .IsRequired(true);
    }
}