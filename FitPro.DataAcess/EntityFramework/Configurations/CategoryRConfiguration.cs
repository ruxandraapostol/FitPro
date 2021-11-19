using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAccess
{
    class CategoryRConfiguration : IEntityTypeConfiguration<CategoryR>
    {
        public void Configure(EntityTypeBuilder<CategoryR> entity)
        {
            entity.HasKey(e => e.IdCategory)
                    .HasName("PK__Category__CBD74706105B55A9");

            entity.ToTable("CategoryR");

            entity.Property(e => e.IdCategory).ValueGeneratedNever();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
