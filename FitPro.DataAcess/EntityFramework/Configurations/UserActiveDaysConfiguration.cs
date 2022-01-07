using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAcess
{
    public class UserActiveDaysConfiguration : IEntityTypeConfiguration<UserActiveDays>
    {
        public void Configure(EntityTypeBuilder<UserActiveDays> entity)
        {
            entity.HasKey(e => new { e.IdRegularUser, e.Date})
                    .HasName("PK__UserActi__ED998A117A9C9DF9");

            entity.ToTable("UserActiveDays");


            entity.Property(e => e.Date).HasColumnType("date");

            entity.HasOne(d => d.IdRegularUserNavigation)
                .WithMany(p => p.UserActiveDays)
                .HasForeignKey(d => d.IdRegularUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserActiveDays_RegularUser");
        }
    }
}
