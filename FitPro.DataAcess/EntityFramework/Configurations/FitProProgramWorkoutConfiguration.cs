using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAccess
{
    public class FitProProgramWorkoutConfiguration : IEntityTypeConfiguration<FitProProgramWorkout>
    {
        public void Configure(EntityTypeBuilder<FitProProgramWorkout> entity)
        {
            entity.HasKey(e => new { e.IdWorkout, e.IdProgram, e.DayNumber })
                    .HasName("PK__FitProPr__AB653D314CA39D5C");

            entity.ToTable("FitProProgram-Workout");

            entity.HasOne(d => d.IdProgramNavigation)
                .WithMany(p => p.FitProProgramWorkouts)
                .HasForeignKey(d => d.IdProgram)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FPW_Program");

            entity.HasOne(d => d.IdWorkoutNavigation)
                .WithMany(p => p.FitProProgramWorkouts)
                .HasForeignKey(d => d.IdWorkout)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PW_Workout");
        }
    }
}