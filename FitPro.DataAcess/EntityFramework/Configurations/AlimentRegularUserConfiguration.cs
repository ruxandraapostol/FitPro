using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAccess
{
    public class AlimentRegularUserConfiguration : IEntityTypeConfiguration<AlimentRegularUser>
    {
        public void Configure(EntityTypeBuilder<AlimentRegularUser> entity)
        {
            entity.HasKey(e => new { e.IdRegularUser, e.IdAliment, e.Date })
                    .HasName("PK__Aliment-__7EDBDF5290088A0B");

            entity.ToTable("Aliment-RegularUser");

            entity.Property(e => e.Date).HasColumnType("date");

            entity.HasOne(d => d.IdAlimentNavigation)
                .WithMany(p => p.AlimentRegularUsers)
                .HasForeignKey(d => d.IdAliment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ARU_Program");

            entity.HasOne(d => d.IdRegularUserNavigation)
                .WithMany(p => p.AlimentRegularUsers)
                .HasForeignKey(d => d.IdRegularUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ARU_RegularUser");
        }
    }
}