using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAccess
{
    public class SpecialUserConfiguration : IEntityTypeConfiguration<SpecialUser>
    {
        public void Configure(EntityTypeBuilder<SpecialUser> entity)
        {
            entity.HasKey(e => e.IdSpecialUser)
                    .HasName("PK__SpecialU__2D8BAD1BC12F8745");

            entity.ToTable("SpecialUser");

            entity.Property(e => e.IdSpecialUser).ValueGeneratedNever();

            entity.HasOne(d => d.IdSpecialUserNavigation)
                .WithOne(p => p.SpecialUser)
                .HasForeignKey<SpecialUser>(d => d.IdSpecialUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserSpecialUser");
        }
    }
}