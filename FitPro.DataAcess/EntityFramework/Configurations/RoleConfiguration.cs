using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAccess
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> entity)
        {
            entity.HasKey(e => e.IdRole)
                    .HasName("PK__Role__B43690544A49D541");

            entity.ToTable("Role");

            entity.Property(e => e.IdRole).ValueGeneratedNever();

            entity.Property(e => e.Description).IsRequired();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}