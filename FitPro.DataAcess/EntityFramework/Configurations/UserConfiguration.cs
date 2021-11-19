using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAccess
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(e => e.IdUser)
                   .HasName("PK__User__B7C926383F28A9F6");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__Recipe__737584F6808CAEC9")
                .IsUnique();

            entity.Property(e => e.IdUser).ValueGeneratedNever();

            entity.Property(e => e.Alive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.IdRole).HasDefaultValueSql("('4EA83F97-6116-44E2-B6EB-437A3BE9C12C')");

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(200);

            entity.HasOne(d => d.IdRoleNavigation)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleUser");
        }
    }
}
