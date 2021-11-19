using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAccess
{
    public class FitProProgramCategoryConfiguration : IEntityTypeConfiguration<FitProProgramCategory>
    {
        public void Configure(EntityTypeBuilder<FitProProgramCategory> entity)
        {
            entity.HasKey(e => new { e.IdProgram, e.IdCategory })
                    .HasName("PK__FitProPr__3DEB07A00E8243E5");

            entity.ToTable("FitProProgram-Category");

            entity.HasOne(d => d.IdCategoryNavigation)
                .WithMany(p => p.FitProProgramCategories)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FPPC_Category");

            entity.HasOne(d => d.IdProgramNavigation)
                .WithMany(p => p.FitProProgramCategories)
                .HasForeignKey(d => d.IdProgram)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FPPC_Program");
        }
    }
}