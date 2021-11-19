using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAccess
{
    public class RegularUserFitProProgramConfiguration : IEntityTypeConfiguration<RegularUserFitProProgram>
    {
        public void Configure(EntityTypeBuilder<RegularUserFitProProgram> entity)
        {
            entity.HasKey(e => new { e.IdRegularUser, e.IdProgram })
                   .HasName("PK__RegularU__8EFF6AFC3EA1C729");

            entity.ToTable("RegularUser-FitProProgram");

            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.Property(e => e.CurrentDay)
                .IsRequired()
                .HasDefaultValueSql("((0))");

            entity.HasOne(d => d.IdProgramNavigation)
                .WithMany(p => p.RegularUserFitProPrograms)
                .HasForeignKey(d => d.IdProgram)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RUFP_Program");

            entity.HasOne(d => d.IdRegularUserNavigation)
                .WithMany(p => p.RegularUserFitProPrograms)
                .HasForeignKey(d => d.IdRegularUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RUP_RegularUser");
        }
    }
}