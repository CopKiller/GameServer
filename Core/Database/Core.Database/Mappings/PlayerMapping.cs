using Core.Database.Constants;
using Core.Database.Models.Account;
using Core.Database.Models.Player;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Database.Mappings;

public class PlayerMapping : IEntityTypeConfiguration<PlayerModel>
{
    public void Configure(EntityTypeBuilder<PlayerModel> builder)
    {
        builder.ToTable("Player");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Name)
            .IsUnique();
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(CharactersLength.MaxNameLength);
        builder.Property(x => x.SlotNumber)
            .IsRequired()
            .HasColumnType("TINYINT");
        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("DATE")
            .HasConversion(
                v => v.ToDateTime(TimeOnly.MinValue), 
                v => DateOnly.FromDateTime(v));
        builder.Property(x => x.LastLogin)
            .IsRequired()
            .HasColumnType("DATE")
            .HasConversion(
                v => v.ToDateTime(TimeOnly.MinValue), 
                v => DateOnly.FromDateTime(v));
        builder.Property(x => x.Level)
            .IsRequired()
            .HasColumnType("INT");
        builder.Property(x => x.Experience)
            .IsRequired()
            .HasColumnType("INT");
        builder.Property(x => x.Golds)
            .IsRequired()
            .HasColumnType("INT");
        builder.Property(x => x.Diamonds)
            .IsRequired()
            .HasColumnType("INT");
        
        builder.OwnsOne(x => x.Vitals, v =>
        {
            v.Property(y => y.Health)
                .IsRequired()
                .HasColumnType("INT");

            v.Property(y => y.Mana)
                .IsRequired()
                .HasColumnType("INT");
        });
        builder.OwnsOne(x => x.Stats, s =>
        {
            s.Property(y => y.Strength)
                .IsRequired()
                .HasColumnType("INT");
            s.Property(y => y.Defense)
                .IsRequired()
                .HasColumnType("INT");
            s.Property(y => y.Agility)
                .IsRequired()
                .HasColumnType("INT");
            s.Property(y => y.Intelligence)
                .IsRequired()
                .HasColumnType("INT");
            s.Property(y => y.Willpower)
                .IsRequired()
                .HasColumnType("INT");
        });
        builder.OwnsOne(x => x.Position, p =>
        {
            p.Property(y => y.X)
                .IsRequired()
                .HasColumnType("FLOAT");
            p.Property(y => y.Y)
                .IsRequired()
                .HasColumnType("FLOAT");
        });
        builder.HasOne(x => x.AccountModel)
            .WithMany(a => a.Players)
            .HasForeignKey(x => x.AccountModelId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
