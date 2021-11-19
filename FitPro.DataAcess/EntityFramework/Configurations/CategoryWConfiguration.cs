using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAccess
{
    public class CategoryWConfiguration : IEntityTypeConfiguration<CategoryW>
    {
        public void Configure(EntityTypeBuilder<CategoryW> entity)
        {
            entity.HasKey(e => e.IdCategory)
                   .HasName("PK__Category__CBD7470685810517");

            entity.ToTable("CategoryW");

            entity.Property(e => e.IdCategory).ValueGeneratedNever();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
