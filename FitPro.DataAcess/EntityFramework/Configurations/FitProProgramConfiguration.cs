using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAccess
{
    public class FitProProgramConfiguration : IEntityTypeConfiguration<FitProProgram>
    {
        public void Configure(EntityTypeBuilder<FitProProgram> entity)
        {
            entity.HasKey(e => e.IdProgram)
                    .HasName("PK__FitProPr__415673D04B5C8159");

            entity.ToTable("FitProProgram");

            entity.Property(e => e.IdProgram).ValueGeneratedNever();
        }
    }
}