using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAccess
{
    public class WorkoutCategoryConfiguration : IEntityTypeConfiguration<WorkoutCategory>
    {
        public void Configure(EntityTypeBuilder<WorkoutCategory> entity)
        {
            entity.HasKey(e => new { e.IdWorkout, e.IdCategory })
                    .HasName("PK__Workout-__C3CD2E7C9CA66D16");

            entity.ToTable("Workout-Category");

            entity.HasOne(d => d.IdCategoryNavigation)
                .WithMany(p => p.WorkoutCategories)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Category");

            entity.HasOne(d => d.IdWorkoutNavigation)
                .WithMany(p => p.WorkoutCategories)
                .HasForeignKey(d => d.IdWorkout)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Workout");
        }
    }
}
