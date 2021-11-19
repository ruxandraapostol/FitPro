using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAccess
{
    public class AlimentConfiguration : IEntityTypeConfiguration<Aliment>
    {
        public void Configure(EntityTypeBuilder<Aliment> entity)
        {
            entity.HasKey(e => e.IdAliment)
                    .HasName("PK__Aliment__346EAEEF37653033");

            entity.ToTable("Aliment");

            entity.Property(e => e.IdAliment).ValueGeneratedNever();

            entity.Property(e => e.Name)
                .IsRequired()
                .IsUnicode(false);

            entity.HasOne(d => d.IdNutritionistNavigation)
                .WithMany(p => p.Aliments)
                .HasForeignKey(d => d.IdNutritionist)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AlimentNutritionist");
        }
    }
}