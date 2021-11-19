using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAccess
{
    public class RegularUserConfiguration : IEntityTypeConfiguration<RegularUser>
    {
        public void Configure(EntityTypeBuilder<RegularUser> entity)
        {
            entity.HasKey(e => e.IdRegularUser)
                    .HasName("PK__RegularU__9AEA0DC1C8D3EDD3");

            entity.ToTable("RegularUser");

            entity.Property(e => e.IdRegularUser).ValueGeneratedNever();

            entity.Property(e => e.BirthDate).HasColumnType("date");

            entity.Property(e => e.Streak).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.IdRegularUserNavigation)
                .WithOne(p => p.RegularUser)
                .HasForeignKey<RegularUser>(d => d.IdRegularUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRegularUser");
        }
    }
}